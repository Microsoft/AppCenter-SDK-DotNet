﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter.Ingestion.Models;
using Microsoft.AppCenter.Ingestion;
using Microsoft.AppCenter.Storage;
using Microsoft.AppCenter.Utils;
using Microsoft.AppCenter.Utils.Synchronization;

namespace Microsoft.AppCenter.Channel
{
    /// <summary>
    /// Default implementation for a channel unit.
    /// </summary>
    public sealed class Channel : IChannelUnit
    {
        private const int ClearBatchSize = 100;
        private Ingestion.Models.Device _device;
        private readonly string _appSecret;
        private readonly IStorage _storage;
        private readonly IIngestion _ingestion;
        private readonly IDeviceInformationHelper _deviceInfoHelper = new DeviceInformationHelper();
        private readonly IDictionary<string, List<Log>> _sendingBatches = new Dictionary<string, List<Log>>();
        private readonly ISet<IServiceCall> _calls = new HashSet<IServiceCall>();
        private readonly int _maxParallelBatches;
        private readonly int _maxLogsPerBatch;
        private long _pendingLogCount;
        private bool _enabled;
        private bool _discardLogs;
        private bool _batchScheduled;
        private TimeSpan _batchTimeInterval;
        private readonly StatefulMutex _mutex = new StatefulMutex();

        internal Channel(string name, int maxLogsPerBatch, TimeSpan batchTimeInterval, int maxParallelBatches,
            string appSecret, IIngestion ingestion, IStorage storage)
        {
            Name = name;
            _maxParallelBatches = maxParallelBatches;
            _maxLogsPerBatch = maxLogsPerBatch;
            _appSecret = appSecret;
            _ingestion = ingestion;
            _storage = storage;
            _batchTimeInterval = batchTimeInterval;
            _batchScheduled = false;
            _enabled = true;
            DeviceInformationHelper.InformationInvalidated += (sender, e) => InvalidateDeviceCache();
            var lockHolder = _mutex.GetLock();
            Task.Run(() => _storage.CountLogsAsync(Name)).ContinueWith(task =>
            {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    _pendingLogCount = task.Result;
                }
                lockHolder.Dispose();
            });
        }

        /// <summary>
        /// Gets value indicating whether the Channel is enabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                using (_mutex.GetLock())
                {
                    return _enabled;
                }
            }
        }

        /// <summary>
        /// The channel's name
        /// </summary>
        public string Name { get; }

        #region Events

        /// <summary>
        /// Invoked when a log will be enqueued.
        /// </summary>
        public event EventHandler<EnqueuingLogEventArgs> EnqueuingLog;

        /// <summary>
        /// Invoked when a log is about to filtered out or not.
        /// </summary>
        public event EventHandler<FilteringLogEventArgs> FilteringLog;

        /// <summary>
        /// Invoke when a log is about to be sent.
        /// </summary>
        public event EventHandler<SendingLogEventArgs> SendingLog;

        /// <summary>
        /// Invoked when a log successfully sent.
        /// </summary>
        public event EventHandler<SentLogEventArgs> SentLog;

        /// <summary>
        /// Invoked when a log failed to send properly.
        /// </summary>
        public event EventHandler<FailedToSendLogEventArgs> FailedToSendLog;

        #endregion

        /// <summary>
        /// Enable or disable this channel unit.
        /// </summary>
        /// <param name="enabled">true to enable, false to disable.</param>
        public void SetEnabled(bool enabled)
        {
            State state;
            using (_mutex.GetLock())
            {
                if (_enabled == enabled)
                {
                    return;
                }
                state = _mutex.State;
            }
            if (enabled)
            {
                Resume(state);
            }
            else
            {
                Suspend(state, true, new CancellationException());
            }
        }

        /// <summary>
        /// Enqueue a log asynchronously.
        /// </summary>
        /// <param name="log">log to enqueue.</param>
        /// <returns>The async Task for this operation.</returns>
        public async Task EnqueueAsync(Log log)
        {
            try
            {
                State state;
                bool discardLogs;
                using (await _mutex.GetLockAsync().ConfigureAwait(false))
                {
                    state = _mutex.State;
                    discardLogs = _discardLogs;
                }
                if (discardLogs)
                {
                    AppCenterLog.Warn(AppCenterLog.LogTag, "Channel is disabled; logs are discarded");
                    AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke SendingLog event for channel '{Name}'");
                    SendingLog?.Invoke(this, new SendingLogEventArgs(log));
                    AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke FailedToSendLog event for channel '{Name}'");
                    FailedToSendLog?.Invoke(this, new FailedToSendLogEventArgs(log, new CancellationException()));
                    return;
                }
                AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke EnqueuingLog event for channel '{Name}'");
                EnqueuingLog?.Invoke(this, new EnqueuingLogEventArgs(log));
                await PrepareLogAsync(log, state).ConfigureAwait(false);
                AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke FilteringLog event for channel '{Name}'");
                var filteringLogEventArgs = new FilteringLogEventArgs(log);
                FilteringLog?.Invoke(this, filteringLogEventArgs);
                if (filteringLogEventArgs.FilterRequested)
                {
                    AppCenterLog.Warn(AppCenterLog.LogTag, $"Filtering out a log of type '{log.GetType()}' at the request of an event handler.");
                }
                else
                {
                    await PersistLogAsync(log, state).ConfigureAwait(false);
                }
            }
            catch (StatefulMutexException)
            {
                AppCenterLog.Warn(AppCenterLog.LogTag, "The Enqueue operation has been cancelled");
            }
        }

        private async Task PrepareLogAsync(Log log, State state)
        {
            if (log.Device == null && _device == null)
            {
                var device = await _deviceInfoHelper.GetDeviceInformationAsync().ConfigureAwait(false);
                using (await _mutex.GetLockAsync(state).ConfigureAwait(false))
                {
                    _device = device;
                }
            }
            log.Device = log.Device ?? _device;
            log.Timestamp = log.Timestamp ?? DateTime.Now;
        }

        private async Task PersistLogAsync(Log log, State state)
        {
            try
            {
                await _storage.PutLog(Name, log).ConfigureAwait(false);
            }
            catch (StorageException e)
            {
                AppCenterLog.Error(AppCenterLog.LogTag, "Error persisting log", e);
                return;
            }
            try
            {
                bool enabled;
                using (await _mutex.GetLockAsync(state).ConfigureAwait(false))
                {
                    _pendingLogCount++;
                    enabled = _enabled;
                }
                if (enabled)
                {
                    CheckPendingLogs(state);
                    return;
                }
                AppCenterLog.Warn(AppCenterLog.LogTag, "Channel is temporarily disabled; log was saved to disk");
            }
            catch (StatefulMutexException)
            {
                AppCenterLog.Warn(AppCenterLog.LogTag, "The PersistLog operation has been cancelled");
            }
        }

        /// <summary>
        /// Invalidate device property cache, meaning next log needing device properties will trigger a re-evaluation of all device properties.
        /// </summary>
        public void InvalidateDeviceCache()
        {
            using (_mutex.GetLock())
            {
                _device = null;
            }
        }

        /// <summary>
        /// Clear all logs that are pending on this unit.
        /// </summary>
        /// <returns>The task to represent this async operation.</returns>
        public async Task ClearAsync()
        {
            var state = _mutex.State;
            await _storage.DeleteLogs(Name).ConfigureAwait(false);
            try
            {
                using (await _mutex.GetLockAsync(state).ConfigureAwait(false))
                {
                    _pendingLogCount = 0;
                }
            }
            catch (StatefulMutexException)
            {
                AppCenterLog.Warn(AppCenterLog.LogTag, "The Clear operation has been cancelled");
            }
        }

        private void Resume(State state)
        {
            AppCenterLog.Verbose(AppCenterLog.LogTag, $"Resume channel: '{Name}'");
            try
            {
                using (_mutex.GetLock(state))
                {
                    _enabled = true;
                    _discardLogs = false;
                    state = _mutex.InvalidateState();
                }
            }
            catch (StatefulMutexException)
            {
                AppCenterLog.Warn(AppCenterLog.LogTag, "The resume operation has been cancelled");
            }
            CheckPendingLogs(state);
        }

        private void Suspend(State state, bool deleteLogs, Exception exception)
        {
            AppCenterLog.Verbose(AppCenterLog.LogTag, $"Suspend channel: '{Name}'");
            try
            {
                List<Log> unsentLogs = null;
                using (_mutex.GetLock(state))
                {
                    _enabled = false;
                    _batchScheduled = false;
                    _discardLogs = deleteLogs;
                    if (deleteLogs)
                    {
                        unsentLogs = _sendingBatches.Values.SelectMany(batch => batch).ToList();
                        _sendingBatches.Clear();
                    }
                    state = _mutex.InvalidateState();
                }
                if (unsentLogs != null && FailedToSendLog != null)
                {
                    foreach (var log in unsentLogs)
                    {
                        AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke FailedToSendLog event for channel '{Name}'");
                        FailedToSendLog?.Invoke(this, new FailedToSendLogEventArgs(log, exception));
                    }
                }
                if (deleteLogs)
                {
                    List<IServiceCall> calls;
                    using (_mutex.GetLock(state))
                    {
                        calls = _calls.ToList();
                        _calls.Clear();
                        _pendingLogCount = 0;
                        TriggerDeleteLogsOnSuspending();
                    }
                    foreach (var call in calls)
                    {
                        call.Cancel();
                    }
                }
                _storage.ClearPendingLogState(Name);
            }
            catch (StatefulMutexException)
            {
                AppCenterLog.Warn(AppCenterLog.LogTag, "The suspend operation has been cancelled");
            }
        }

        private void TriggerDeleteLogsOnSuspending()
        {
            if (SendingLog == null && FailedToSendLog == null)
            {
                _storage.DeleteLogs(Name);
                return;
            }
            SignalDeletingLogs().ContinueWith(completedTask => _storage.DeleteLogs(Name));
        }

        private Task SignalDeletingLogs()
        {
            var logs = new List<Log>();
            return _storage.GetLogsAsync(Name, ClearBatchSize, logs)
                .ContinueWith(completedTask =>
                {
                    if (completedTask.IsFaulted)
                    {
                        AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to invoke events for logs being deleted.");
                        return;
                    }
                    foreach (var log in logs)
                    {
                        AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke SendingLog for channel '{Name}'");
                        SendingLog?.Invoke(this, new SendingLogEventArgs(log));
                        AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke FailedToSendLog event for channel '{Name}'");
                        FailedToSendLog?.Invoke(this, new FailedToSendLogEventArgs(log, new CancellationException()));
                    }
                    if (logs.Count >= ClearBatchSize)
                    {
                        SignalDeletingLogs();
                    }
                });
        }

        private async Task TriggerIngestionAsync(State state)
        {
            using (await _mutex.GetLockAsync(state).ConfigureAwait(false))
            {
                if (!_enabled || !_batchScheduled)
                {
                    return;
                }
                AppCenterLog.Debug(AppCenterLog.LogTag,
                    $"TriggerIngestion({Name}) pending log count: {_pendingLogCount}");
                _batchScheduled = false;
                if (_sendingBatches.Count >= _maxParallelBatches)
                {
                    AppCenterLog.Debug(AppCenterLog.LogTag,
                        $"Already sending {_maxParallelBatches} batches of analytics data to the server");
                    return;
                }
            }

            // Get a batch from storage
            var logs = new List<Log>();
            var batchId = await _storage.GetLogsAsync(Name, _maxLogsPerBatch, logs).ConfigureAwait(false);
            if (batchId != null)
            {
                using (await _mutex.GetLockAsync(state).ConfigureAwait(false))
                {
                    _sendingBatches.Add(batchId, logs);
                    _pendingLogCount -= logs.Count;
                }
                try
                {
                    // Before sending logs, trigger the sending event for this channel
                    if (SendingLog != null)
                    {
                        foreach (var log in logs)
                        {
                            AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke SendingLog event for channel '{Name}'");
                            SendingLog?.Invoke(this, new SendingLogEventArgs(log));
                        }
                    }

                    // If the optional Install ID has no value, default to using empty GUID
                    var installId = await AppCenter.GetInstallIdAsync().ConfigureAwait(false) ?? Guid.Empty;
                    var ingestionCall = _ingestion.Call(_appSecret, installId, logs);
                    using (await _mutex.GetLockAsync(state).ConfigureAwait(false))
                    {
                        _calls.Add(ingestionCall);
                    }
                    ingestionCall.ContinueWith(call => HandleSendingResult(state, batchId, call));
                    CheckPendingLogs(state);
                }
                catch (StorageException)
                {
                    AppCenterLog.Warn(AppCenterLog.LogTag, "Something went wrong sending logs to ingestion");
                }
            }
        }

        private void HandleSendingResult(State state, string batchId, IServiceCall call)
        {
            // Get a lock without checking the state here.
            using (_mutex.GetLock())
            {
                _calls.Remove(call);
            }
            if (call.IsCanceled)
            {
                HandleSendingCancel(state, batchId);
            }
            else if (call.IsFaulted)
            {
                HandleSendingFailure(state, batchId, call.Exception);
            }
            else
            {
                HandleSendingSuccess(state, batchId);
            }
        }

        private void HandleSendingSuccess(State state, string batchId)
        {
            if (!_mutex.IsCurrent(state))
            {
                return;
            }
            try
            {
                _storage.DeleteLogs(Name, batchId);
            }
            catch (StorageException e)
            {
                AppCenterLog.Warn(AppCenterLog.LogTag, $"Could not delete logs for batch {batchId}", e);
            }
            finally
            {
                List<Log> removedLogs;
                using (_mutex.GetLock(state))
                {
                    removedLogs = _sendingBatches[batchId];
                    _sendingBatches.Remove(batchId);
                }
                if (SentLog != null)
                {
                    foreach (var log in removedLogs)
                    {
                        AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke SentLog event for channel '{Name}'");
                        SentLog?.Invoke(this, new SentLogEventArgs(log));
                    }
                }
            }
        }

        private void HandleSendingCancel(State state, string batchId)
        {
            AppCenterLog.Debug(AppCenterLog.LogTag, $"Sending logs for channel '{Name}', batch '{batchId}' canceled");
            try
            {
                List<Log> removedLogs;
                using (_mutex.GetLock(state))
                {
                    removedLogs = _sendingBatches[batchId];
                    _sendingBatches.Remove(batchId);
                }
                if (FailedToSendLog != null)
                {
                    foreach (var log in removedLogs)
                    {
                        AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke FailedToSendLog event for channel '{Name}'");
                        FailedToSendLog?.Invoke(this, new FailedToSendLogEventArgs(log, new CancellationException()));
                    }
                }
            }
            catch (StatefulMutexException)
            {
                AppCenterLog.Debug(AppCenterLog.LogTag,
                    "Handle sending cancel operation has been canceled. Callbacks were invoked when channel suspended.");
            }
        }

        private void HandleSendingFailure(State state, string batchId, Exception e)
        {
            AppCenterLog.Error(AppCenterLog.LogTag, $"Sending logs for channel '{Name}', batch '{batchId}' failed: {e?.Message}");
            try
            {
                var isRecoverable = e is IngestionException ingestionException && ingestionException.IsRecoverable;
                List<Log> removedLogs;
                using (_mutex.GetLock(state))
                {
                    removedLogs = _sendingBatches[batchId];
                    _sendingBatches.Remove(batchId);
                    if (isRecoverable)
                    {
                        _pendingLogCount += removedLogs.Count;
                    }
                }
                if (!isRecoverable && FailedToSendLog != null)
                {
                    foreach (var log in removedLogs)
                    {
                        AppCenterLog.Verbose(AppCenterLog.LogTag, $"Invoke FailedToSendLog event for channel '{Name}'");
                        FailedToSendLog?.Invoke(this, new FailedToSendLogEventArgs(log, e));
                    }
                }
                Suspend(state, !isRecoverable, e);
            }
            catch (StatefulMutexException)
            {
                AppCenterLog.Debug(AppCenterLog.LogTag,
                    "Handle sending failure operation has been canceled. Callbacks were invoked when channel suspended.");
            }
        }

        private void CheckPendingLogs(State state)
        {
            if (!_enabled)
            {
                AppCenterLog.Info(AppCenterLog.LogTag, "The service has been disabled. Stop processing logs.");
                return;
            }

            AppCenterLog.Debug(AppCenterLog.LogTag, $"CheckPendingLogs({Name}) pending log count: {_pendingLogCount}");
            using (_mutex.GetLock())
            {
                if (_pendingLogCount >= _maxLogsPerBatch)
                {
                    _batchScheduled = true;
                    Task.Run(async () =>
                    {
                        await TriggerIngestionAsync(state).ConfigureAwait(false);
                    });
                }
                else if (_pendingLogCount > 0 && !_batchScheduled)
                {
                    _batchScheduled = true;

                    // No need wait _batchTimeInterval here.
                    Task.Run(async () =>
                    {
                        await Task.Delay((int)_batchTimeInterval.TotalMilliseconds).ConfigureAwait(false);
                        if (_batchScheduled)
                        {
                            await TriggerIngestionAsync(_mutex.State).ConfigureAwait(false);
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Stop all calls in progress and deactivate this channel.
        /// </summary>
        /// <returns>The Task to represent this async operation.</returns>
        public Task ShutdownAsync()
        {
            Suspend(_mutex.State, false, new CancellationException());

            // Nothing to wait on; just suspend and return a task
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// Free resources held by this instance.
        /// </summary>
        public void Dispose()
        {
            _mutex.Dispose();
        }
    }
}
