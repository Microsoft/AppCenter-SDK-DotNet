// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foundation;
using Microsoft.AppCenter.Crashes.iOS.Bindings;

namespace Microsoft.AppCenter.Crashes
{
    using iOSCrashes = iOS.Bindings.MSACCrashes;

    public partial class Crashes
    {
        /// <summary>
        /// Internal SDK property not intended for public use.
        /// </summary>
        /// <value>
        /// The iOS SDK Crashes bindings type.
        /// </value>
        [Preserve]
        public static Type BindingType => typeof(iOSCrashes);

        static Task<bool> PlatformIsEnabledAsync()
        {
            return Task.FromResult(iOSCrashes.IsEnabled());
        }

        static Task PlatformSetEnabledAsync(bool enabled)
        {
            iOSCrashes.SetEnabled(enabled);
            return Task.FromResult(default(object));
        }

        static Task<bool> PlatformHasCrashedInLastSessionAsync()
        {
            return Task.FromResult(iOSCrashes.HasCrashedInLastSession);
        }

        static Task<ErrorReport> PlatformGetLastSessionCrashReportAsync()
        {
            return Task.Run(() =>
            {
                var msReport = iOSCrashes.LastSessionCrashReport;
                return (msReport == null) ? null : new ErrorReport(msReport);
            });
        }

        static Task<bool> PlatformHasReceivedMemoryWarningInLastSessionAsync()
        {
            return Task.FromResult(iOSCrashes.HasReceivedMemoryWarningInLastSession);
        }

        static void PlatformNotifyUserConfirmation(UserConfirmation confirmation)
        {
            MSACUserConfirmation iosUserConfirmation;
            switch (confirmation)
            {
                case UserConfirmation.Send:
                    iosUserConfirmation = MSACUserConfirmation.Send;
                    break;
                case UserConfirmation.DontSend:
                    iosUserConfirmation = MSACUserConfirmation.DontSend;
                    break;
                case UserConfirmation.AlwaysSend:
                    iosUserConfirmation = MSACUserConfirmation.Always;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(confirmation), confirmation, null);
            }
            iOSCrashes.NotifyWithUserConfirmation(iosUserConfirmation);
        }

        static void PlatformTrackError(Exception exception, IDictionary<string, string> properties, ErrorAttachmentLog[] attachments)
        {
            NSDictionary propertyDictionary = properties != null ? StringDictToNSDict(properties) : new NSDictionary();
            NSMutableArray attachmentArray = new NSMutableArray();
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    if (attachment?.internalAttachment != null)
                    {
                        attachmentArray.Add(attachment.internalAttachment);
                    }
                    else
                    {
                        AppCenterLog.Warn(LogTag, "Skipping null ErrorAttachmentLog in Crashes.TrackError.");
                    }
                }
            }
            MSACCrashes.TrackException(GenerateiOSException(exception, false), propertyDictionary, attachmentArray);
        }

        /// <summary>
        /// We keep the reference to avoid it being freed, inlining this object will cause listeners not to be called.
        /// </summary>
        static readonly CrashesInitializationDelegate _crashesInitializationDelegate = new CrashesInitializationDelegate();

        /// <summary>
        /// We keep the reference to avoid it being freed, inlining this object will cause listeners not to be called.
        /// </summary>
        static readonly CrashesDelegate _crashesDelegate = new CrashesDelegate();

        static Crashes()
        {
            /* Perform custom setup around the native SDK's for setting signal handlers */
            iOSCrashes.DisableMachExceptionHandler();
            MSACWrapperCrashesHelper.SetCrashHandlerSetupDelegate(_crashesInitializationDelegate);
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            iOSCrashes.SetUserConfirmationHandler((reports) =>
                    {
                        if (ShouldAwaitUserConfirmation != null)
                        {
                            return ShouldAwaitUserConfirmation();
                        }
                        return false;
                    });
            iOSCrashes.SetDelegate(_crashesDelegate);
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var systemException = e.ExceptionObject as Exception;
            AppCenterLog.Error(LogTag, "Unhandled Exception:", systemException);
            var exception = GenerateiOSException(systemException, true);
            var exceptionBytes = CrashesUtils.SerializeException(systemException) ?? new byte[0];
            var wrapperExceptionData = NSData.FromArray(exceptionBytes);
            var wrapperException = new MSACWrapperException
            {
                Exception = exception,
                ExceptionData = wrapperExceptionData,
                ProcessId = new NSNumber(Process.GetCurrentProcess().Id)
            };
            AppCenterLog.Info(LogTag, "Saving wrapper exception...");
            MSACWrapperExceptionManager.SaveWrapperException(wrapperException);
            AppCenterLog.Info(LogTag, "Saved wrapper exception.");
        }

        static MSACWrapperExceptionModel GenerateiOSException(Exception exception, bool structuredFrames)
        {
            var msException = new MSACWrapperExceptionModel();
            msException.Type = exception.GetType().FullName;
            msException.Message = exception.Message ?? "";
            msException.StackTrace = exception.StackTrace;
            msException.Frames = structuredFrames ? GenerateStackFrames(exception) : null;
            msException.WrapperSdkName = WrapperSdk.Name;

            var aggregateException = exception as AggregateException;
            var innerExceptions = new List<MSACWrapperExceptionModel>();

            if (aggregateException?.InnerExceptions != null)
            {
                foreach (Exception innerException in aggregateException.InnerExceptions)
                {
                    innerExceptions.Add(GenerateiOSException(innerException, structuredFrames));
                }
            }
            else if (exception.InnerException != null)
            {
                innerExceptions.Add(GenerateiOSException(exception.InnerException, structuredFrames));
            }

            msException.InnerExceptions = innerExceptions.Count > 0 ? innerExceptions.ToArray() : null;

            return msException;
        }

#pragma warning disable XS0001 // Find usages of mono todo items

        static MSACStackFrame[] GenerateStackFrames(Exception e)
        {
            var trace = new StackTrace(e, true);
            var frameList = new List<MSACStackFrame>();

            for (int i = 0; i < trace.FrameCount; ++i)
            {
                StackFrame dotnetFrame = trace.GetFrame(i);
                if (dotnetFrame.GetMethod() == null) continue;
                var msFrame = new MSACStackFrame();
                msFrame.Address = null;
                msFrame.Code = null;
                msFrame.MethodName = dotnetFrame.GetMethod().Name;
                msFrame.ClassName = dotnetFrame.GetMethod().DeclaringType?.FullName;
                msFrame.LineNumber = dotnetFrame.GetFileLineNumber() == 0 ? null : (NSNumber)(dotnetFrame.GetFileLineNumber());
                msFrame.FileName = AnonymizePath(dotnetFrame.GetFileName());
                frameList.Add(msFrame);
            }
            return frameList.ToArray();
        }

#pragma warning restore XS0001 // Find usages of mono todo items

        static string AnonymizePath(string path)
        {
            if ((path == null) || (path.Count() == 0) || !path.Contains("/Users/"))
            {
                return path;
            }

            string pattern = "(/Users/[^/]+/)";
            return Regex.Replace(path, pattern, "/Users/USER/");
        }

        // Bridge between .NET events/callbacks and Apple native SDK
        class CrashesDelegate : MSACCrashesDelegate
        {
            public override bool CrashesShouldProcessErrorReport(iOSCrashes crashes, MSACErrorReport msReport)
            {
                if (ShouldProcessErrorReport == null)
                {
                    return true;
                }
                var report = new ErrorReport(msReport);
                return ShouldProcessErrorReport(report);
            }

            public override NSArray AttachmentsWithCrashes(iOSCrashes crashes, MSACErrorReport msReport)
            {
                if (GetErrorAttachments == null)
                {
                    return null;
                }
                var report = new ErrorReport(msReport);
                var attachments = GetErrorAttachments(report);
                if (attachments != null)
                {
                    var nsArray = new NSMutableArray();
                    foreach (var attachment in attachments)
                    {
                        if (attachment != null)
                        {
                            nsArray.Add(attachment.internalAttachment);
                        }
                        else
                        {
                            AppCenterLog.Warn(LogTag, "Skipping null ErrorAttachmentLog in Crashes.GetErrorAttachments.");
                        }
                    }
                    return nsArray;
                }
                return null;
            }

            public override void CrashesWillSendErrorReport(iOSCrashes crashes, MSACErrorReport msReport)
            {
                if (SendingErrorReport != null)
                {
                    var report = new ErrorReport(msReport);
                    var e = new SendingErrorReportEventArgs
                    {
                        Report = report
                    };
                    SendingErrorReport(null, e);
                }
            }

            public override void CrashesDidSucceedSendingErrorReport(iOSCrashes crashes, MSACErrorReport msReport)
            {
                if (SentErrorReport != null)
                {
                    var report = new ErrorReport(msReport);
                    var e = new SentErrorReportEventArgs
                    {
                        Report = report
                    };
                    SentErrorReport(null, e);
                }

            }

            public override void CrashesDidFailSendingErrorReport(iOSCrashes crashes, MSACErrorReport msReport, NSError error)
            {
                if (FailedToSendErrorReport != null)
                {
                    var report = new ErrorReport(msReport);
                    var e = new FailedToSendErrorReportEventArgs
                    {
                        Report = report,
                        Exception = error
                    };
                    FailedToSendErrorReport(null, e);
                }
            }
        }

        private static NSDictionary StringDictToNSDict(IDictionary<string, string> dict)
        {
            return NSDictionary.FromObjectsAndKeys(dict.Values.ToArray(), dict.Keys.ToArray());
        }

        private static void PlatformUnsetInstance()
        {
            iOSCrashes.ResetSharedInstance();
        }

        public static bool UseMonoRuntimeSignalMethods { get; set; } = true;
    }
}
