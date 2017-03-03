﻿using Microsoft.Azure.Mobile.Channel;
using Microsoft.Azure.Mobile.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Mobile
{
    /// <summary>
    /// SDK core used to initialize, start and control specific service.
    /// </summary>
    public partial class MobileCenter
    {
        private const string EnabledKey = "MobileCenterEnabled";
        private ChannelGroup _channelGroup;
        private readonly HashSet<IMobileCenterService> _services = new HashSet<IMobileCenterService>();
        private string _serverUrl;
        private readonly static object MobileCenterLock = new object();
        private readonly static IApplicationSettings ApplicationSettings = new ApplicationSettings();
        private static bool _logLevelSet;

        #region static

        private static MobileCenter _instanceField;

        internal static MobileCenter Instance
        {
            get
            {
                lock (MobileCenterLock)
                {
                    return _instanceField ?? (_instanceField = new MobileCenter());
                }
            }
            set
            {
                lock (MobileCenterLock)
                {
                    _instanceField = value;
                }
            }
        }

        /// <summary>
        ///     This property controls the amount of logs emitted by the SDK.
        /// </summary>
        public static LogLevel LogLevel
        {
            get
            {
                lock (MobileCenterLock)
                {
                    return MobileCenterLog.Level;
                }
            }
            set
            {
                lock (MobileCenterLock)
                {
                    MobileCenterLog.Level = value;
                    _logLevelSet = true;
                }
            }
        }

        /// <summary>
        ///     Enable or disable the SDK as a whole. Updating the property propagates the value to all services that have been
        ///     started.
        /// </summary>
        /// <remarks>
        ///     The default state is <c>true</c> and updating the state is persisted into local application storage.
        /// </remarks>
        public static bool Enabled
        {
            get
            {
                lock (MobileCenterLock)
                {
                    return Instance.InstanceEnabled;
                }
            }
            set
            {
                lock (MobileCenterLock)
                {
                    Instance.InstanceEnabled = value;
                }
            }
        }

        /// <summary>
        ///     Get the unique installation identifier for this application installation on this device.
        /// </summary>
        /// <remarks>
        ///     The identifier is lost if clearing application data or uninstalling application.
        /// </remarks>
        public static Guid? InstallId => IdHelper.InstallId;

        /// <summary>
        ///     Change the base URL (scheme + authority + port only) used to communicate with the backend.
        /// </summary>
        /// <param name="serverUrl">Base URL to use for server communication.</param>
        public static void SetServerUrl(string serverUrl)
        {
            lock (MobileCenterLock)
            {
                Instance.SetInstanceServerUrl(serverUrl);
            }
        }

        /// <summary>
        /// Check whether SDK has already been configured or not.
        /// </summary>
        public static bool Configured
        {
            get
            {
                lock (MobileCenterLock)
                {
                    return Instance._instanceConfigured;
                }
            }
        }

        /// <summary>
        ///     Configure the SDK.
        ///     This may be called only once per application process lifetime.
        /// </summary>
        /// <param name="appSecret">A unique and secret key used to identify the application.</param>
        public static void Configure(string appSecret)
        {
            lock (MobileCenterLock)
            {
                Instance.InstanceConfigure(appSecret);
            }
        }

        /// <summary>
        ///     Start services.
        ///     This may be called only once per service per application process lifetime.
        /// </summary>
        /// <param name="services">List of services to use.</param>
        public static void Start(params Type[] services)
        {
            lock (MobileCenterLock)
            {
                Instance.StartInstance(services);
            }
        }

        /// <summary>
        ///     Initialize the SDK with the list of services to start.
        ///     This may be called only once per application process lifetime.
        /// </summary>
        /// <param name="appSecret">A unique and secret key used to identify the application.</param>
        /// <param name="services">List of services to use.</param>
        public static void Start(string appSecret, params Type[] services)
        {
            lock (MobileCenterLock)
            {
                Instance.StartInstance(appSecret, services);
            }
        }
        #endregion

        #region instance

        private bool InstanceEnabled
        {
            get
            {
                return ApplicationSettings.GetValue(EnabledKey, true);
            }
            set
            {
                _channelGroup?.SetEnabled(value);
                var previouslyEnabled = InstanceEnabled;
                var switchToDisabled = previouslyEnabled && !value;
                var switchToEnabled = !previouslyEnabled && value;
                ApplicationSettings[EnabledKey] = value;

                foreach (var service in _services)
                {
                    service.InstanceEnabled = value;
                }

                if (switchToDisabled)
                {
                    MobileCenterLog.Info(MobileCenterLog.LogTag, "Mobile Center has been disabled.");
                }
                else if (switchToEnabled)
                {
                    MobileCenterLog.Info(MobileCenterLog.LogTag, "Mobile Center has been enabled.");
                }
                else
                {
                    MobileCenterLog.Info(MobileCenterLog.LogTag, "Mobile Center has already been " + (value ? "enabled." : "disabled."));
                }
            }
        }

        private void SetInstanceServerUrl(string serverUrl)
        {
            _serverUrl = serverUrl;
            _channelGroup?.SetServerUrl(serverUrl);
        }

        private bool _instanceConfigured;

        private bool InstanceConfigure(string appSecret)
        {
            if (!_logLevelSet)
            {
                MobileCenterLog.Level = LogLevel.Warn;
                _logLevelSet = true;
            }
            if (_instanceConfigured)
            {
                MobileCenterLog.Error(MobileCenterLog.LogTag, "Mobile Center may only be configured once");
            }
            else if (string.IsNullOrEmpty(appSecret))
            {
                MobileCenterLog.Error(MobileCenterLog.LogTag, "appSecret may not be null or empty");
            }
            else
            {
                _channelGroup = new ChannelGroup(appSecret);
                //TODO what if disabled here but enabled when starting service? its possible to have conflicts
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    MobileCenterLog.Assert("Zander", "waiting");
                    Task.Delay(10000).Wait();
                    MobileCenterLog.Assert("Zander", "done waiting");

                };
                throw new Exception();
                if (_serverUrl != null)
                {
                    _channelGroup.SetServerUrl(_serverUrl);
                }
                _instanceConfigured = true;
                MobileCenterLog.Assert(MobileCenterLog.LogTag, "Mobile Center SDK configured successfully.");
                return true;
            }
            MobileCenterLog.Assert(MobileCenterLog.LogTag, "Mobile Center SDK configuration failed.");
            return false;
        }

        private void StartInstance(params Type[] services)
        {
            if (services == null)
            {
                MobileCenterLog.Error(MobileCenterLog.LogTag, "Cannot start services; services array is null. Failed to start services.");
                return;
            }

            if (!_instanceConfigured)
            {
                var serviceNames = services.Aggregate("", (current, serviceType) => current + $"\t{serviceType.Name}\n");
                MobileCenterLog.Error(MobileCenterLog.LogTag, "Cannot start services; Mobile Center has not been configured. Failed to start the following services:\n" + serviceNames);
                return;
            }

            foreach (var serviceType in services)
            {
                if (serviceType == null)
                {
                    MobileCenterLog.Warn(MobileCenterLog.LogTag, "Skipping null service. Please check that you did not pass a null argument.");
                    continue;
                }
                try
                {
                    var serviceInstance = (IMobileCenterService)serviceType.GetRuntimeProperty("Instance")?.GetValue(null);
                    if (serviceInstance == null)
                    {
                        throw new MobileCenterException("Service type must contain static 'Instance' property");
                    }
                    StartService(serviceInstance);
                }
                catch (MobileCenterException ex)
                {
                    MobileCenterLog.Error(MobileCenterLog.LogTag, $"Failed to start service '{serviceType.Name}'; skipping it.", ex);
                }
            }
        }

        private void StartService(IMobileCenterService service)
        {
            if (_services.Contains(service))
            {
                MobileCenterLog.Warn(MobileCenterLog.LogTag, $"Mobile Center has already started a service of type '{service.GetType().Name}'.");
                return;
            }
            _services.Add(service);
            service.OnChannelGroupReady(_channelGroup);
            MobileCenterLog.Info(MobileCenterLog.LogTag, $"'{service.GetType().Name}' service started.");
        }

        public void StartInstance(string appSecret, params Type[] services)
        {
            try
            {
                var parsedSecret = GetSecretForPlatform(appSecret, PlatformIdentifier);
                if (InstanceConfigure(parsedSecret))
                {
                    StartInstance(services);
                }
            }
            catch (ArgumentException ex)
            {
                MobileCenterLog.Assert(MobileCenterLog.LogTag, ex.Message);
            }
        }
        #endregion
    }
}