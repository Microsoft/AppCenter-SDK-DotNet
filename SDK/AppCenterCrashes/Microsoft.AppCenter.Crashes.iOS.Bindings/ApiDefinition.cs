// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Foundation;
using ObjCRuntime;
using CoreFoundation;

namespace Microsoft.AppCenter.Crashes.iOS.Bindings
{
    // @interface MSACErrorReport : NSObject
    [BaseType(typeof(NSObject))]
    interface MSACErrorReport
    {
        // @property (readonly, nonatomic) NSString * incidentIdentifier;
        [Export("incidentIdentifier")]
        string IncidentIdentifier { get; }

        // @property (readonly, nonatomic) NSString * reporterKey;
        [Export("reporterKey")]
        string ReporterKey { get; }

        // @property (readonly, nonatomic) NSString * signal;
        [Export("signal")]
        string Signal { get; }

        // @property (readonly, nonatomic) NSString * exceptionName;
        [Export("exceptionName")]
        string ExceptionName { get; }

        // @property (readonly, nonatomic) NSString * exceptionReason;
        [Export("exceptionReason")]
        string ExceptionReason { get; }

        // @property (readonly, nonatomic, strong) NSDate * appStartTime;
        [Export("appStartTime", ArgumentSemantic.Strong)]
        NSDate AppStartTime { get; }

        // @property (readonly, nonatomic, strong) NSDate * appErrorTime;
        [Export("appErrorTime", ArgumentSemantic.Strong)]
        NSDate AppErrorTime { get; }

        // @property (readonly, nonatomic) MSACDevice * device;
        [Export("device")]
        Microsoft.AppCenter.iOS.Bindings.MSACDevice Device { get; }

        // @property (readonly, assign, nonatomic) NSUInteger appProcessIdentifier;
        [Export("appProcessIdentifier")]
        nuint AppProcessIdentifier { get; }

        // -(BOOL)isAppKill;
        [Export("isAppKill")]
        //[Verify(MethodToProperty)]
        bool IsAppKill { get; }
    }

    // typedef bool (^MSACUserConfirmationHandler)(NSArray<MSACErrorReport *> * _Nonnull);
    delegate bool MSACUserConfirmationHandler(MSACErrorReport[] reports);

    // @interface MSACCrashes
    [BaseType(typeof(NSObject))]
    interface MSACCrashes
    {
        // +(void)setEnabled:(BOOL)isEnabled;
        [Static]
        [Export("setEnabled:")]
        void SetEnabled(bool isEnabled);

        // +(BOOL)isEnabled;
        [Static]
        [Export("isEnabled")]
        bool IsEnabled();

        // +(void)generateTestCrash;
        [Static]
        [Export("generateTestCrash")]
        void GenerateTestCrash();

        //+(void)trackException:(MSACExceptionModel *)exception withProperties:(NSDictionary *)properties attachments:(nullable NSArray<MSACErrorAttachmentLog *> *)attachments;
        [Static]
        [Export("trackException:withProperties:attachments:")]
        void TrackException(MSACExceptionModel exception, NSDictionary properties, NSArray attachments);

        //(BOOL)hasCrashedInLastSession;
        [Static]
        [Export("hasCrashedInLastSession")]
        bool HasCrashedInLastSession { get; }

        //(BOOL)hasReceivedMemoryWarningInLastSession;
        [Static]
        [Export("hasReceivedMemoryWarningInLastSession")]
        bool HasReceivedMemoryWarningInLastSession { get; }

        //(MSACErrorReport * _Nullable)lastSessionCrashReport;
        [Static]
        [NullAllowed, Export("lastSessionCrashReport")]
        MSACErrorReport LastSessionCrashReport { get; }

        //(void)setDelegate:(id<MSACCrashesDelegate> _Nullable)delegate;
        [Static]
        [Export("setDelegate:")]
        void SetDelegate([NullAllowed] MSACCrashesDelegate crashesDelegate);

        //(void)setUserConfirmationHandler:(MSACUserConfirmationHandler _Nullable)userConfirmationHandler;
        [Static]
        [Export("setUserConfirmationHandler:")]
        void SetUserConfirmationHandler([NullAllowed] MSACUserConfirmationHandler userConfirmationHandler);

        //(void)notifyWithUserConfirmation:(MSACUserConfirmation)userConfirmation;
        [Static]
        [Export("notifyWithUserConfirmation:")]
        void NotifyWithUserConfirmation(MSACUserConfirmation userConfirmation);

        //+(void)disableMachExceptionHandler;
        [Static]
        [Export("disableMachExceptionHandler")]
        void DisableMachExceptionHandler();

        [Static]
        [Export("resetSharedInstance")]
        void ResetSharedInstance();
    }

    // @protocol MSACCrashesDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface MSACCrashesDelegate
    {
        // @optional -(BOOL)crashes:(MSACCrashes *)crashes shouldProcessErrorReport:(MSACErrorReport *)errorReport;
        [Export("crashes:shouldProcessErrorReport:")]
        bool CrashesShouldProcessErrorReport(MSACCrashes crashes, MSACErrorReport errorReport);

        // @optional - (NSArray<MSACErrorAttachmentLog *> *)attachmentsWithCrashes:(MSACCrashes *)crashes forErrorReport:(MSACErrorReport *)errorReport;
        [Export("attachmentsWithCrashes:forErrorReport:")]
        NSArray AttachmentsWithCrashes(MSACCrashes crashes, MSACErrorReport errorReport);

        // @optional -(void)crashes:(MSACCrashes *)crashes willSendErrorReport:(MSACErrorReport *)errorReport;
        [Export("crashes:willSendErrorReport:")]
        void CrashesWillSendErrorReport(MSACCrashes crashes, MSACErrorReport errorReport);

        // @optional -(void)crashes:(MSACCrashes *)crashes didSucceedSendingErrorReport:(MSACErrorReport *)errorReport;
        [Export("crashes:didSucceedSendingErrorReport:")]
        void CrashesDidSucceedSendingErrorReport(MSACCrashes crashes, MSACErrorReport errorReport);

        // @optional -(void)crashes:(MSACCrashes *)crashes didFailSendingErrorReport:(MSACErrorReport *)errorReport withError:(NSError *)error;
        [Export("crashes:didFailSendingErrorReport:withError:")]
        void CrashesDidFailSendingErrorReport(MSACCrashes crashes, MSACErrorReport errorReport, NSError error);
    }

    // @interface MSACErrorAttachmentLog : NSObject
    [BaseType(typeof(NSObject))]
    interface MSACErrorAttachmentLog
    {
        // + (MSACErrorAttachmentLog *)attachmentWithText:(NSString *)text filename:(NSString *)filename;
        [Static]
        [Export("attachmentWithText:filename:")]
        MSACErrorAttachmentLog AttachmentWithText([NullAllowed] string text, [NullAllowed] string fileName);

        // + (MSACErrorAttachmentLog *)attachmentWithBinary:(NSData *)data filename:(NSString*)filename contentType:(NSString*)contentType;
        [Static]
        [Export("attachmentWithBinary:filename:contentType:")]
        MSACErrorAttachmentLog AttachmentWithBinaryData(NSData data, [NullAllowed] string filename, string contentType);
    }

    // @interface MSACExceptionModel : NSObject
    [BaseType(typeof(NSObject))]
    interface MSACExceptionModel
    {
        // @property (nonatomic) NSString * _Nonnull type;
        [Export("type")]
        string Type { get; set; }

        // @property (nonatomic) NSString * _Nonnull message;
        [Export("message")]
        string Message { get; set; }

        // @property (nonatomic) NSString * _Nullable stackTrace;
        [NullAllowed, Export("stackTrace")]
        string StackTrace { get; set; }

        // @property (nonatomic) NSArray<MSACStackFrame *> * _Nullable frames;
        [NullAllowed, Export("frames", ArgumentSemantic.Assign)]
        MSACStackFrame[] Frames { get; set; }
    }

    // @interface MSACWrapperExceptionModel : MSACExceptionModel
    [BaseType(typeof(MSACExceptionModel))]
    interface MSACWrapperExceptionModel
    {
        // @property (nonatomic) NSArray<MSACException *> * _Nullable innerExceptions;
        [NullAllowed, Export("innerExceptions", ArgumentSemantic.Assign)]
        MSACWrapperExceptionModel[] InnerExceptions { get; set; }

        // @property (nonatomic) NSString * _Nullable wrapperSdkName;
        [NullAllowed, Export("wrapperSdkName")]
        string WrapperSdkName { get; set; }

        // -(BOOL)isEqual:(MSACWrapperExceptionModel * _Nullable)exception;
        [Export("isEqual:")]
        bool IsEqual([NullAllowed] MSACWrapperExceptionModel exception);
    }

    // @interface MSACStackFrame : NSObject
    [BaseType(typeof(NSObject))]
    interface MSACStackFrame
    {
        // @property (nonatomic) NSString * _Nullable address;
        [NullAllowed, Export("address")]
        string Address { get; set; }

        // @property (nonatomic) NSString * _Nullable code;
        [NullAllowed, Export("code")]
        string Code { get; set; }

        // @property (nonatomic) NSString * _Nullable className;
        [NullAllowed, Export("className")]
        string ClassName { get; set; }

        // @property (nonatomic) NSString * _Nullable methodName;
        [NullAllowed, Export("methodName")]
        string MethodName { get; set; }

        // @property (nonatomic) NSNumber * _Nullable lineNumber;
        [NullAllowed, Export("lineNumber", ArgumentSemantic.Assign)]
        NSNumber LineNumber { get; set; }

        // @property (nonatomic) NSString * _Nullable fileName;
        [NullAllowed, Export("fileName")]
        string FileName { get; set; }

        // -(BOOL)isEqual:(MSACStackFrame * _Nullable)frame;
        [Export("isEqual:")]
        bool IsEqual([NullAllowed] MSACStackFrame frame);
    }


    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface MSACCrashHandlerSetupDelegate
    {
        //- (void) willSetUpCrashHandlers;
        [Export("willSetUpCrashHandlers")]
        void WillSetUpCrashHandlers();

        //- (void) didSetUpCrashHandlers;
        [Export("didSetUpCrashHandlers")]
        void DidSetUpCrashHandlers();

        //- (BOOL) shouldEnableUncaughtExceptionHandler;
        [Export("shouldEnableUncaughtExceptionHandler")]
        bool ShouldEnableUncaughtExceptionHandler();
    }

    // @interface MSACWrapperExceptionManager : NSObject
    [BaseType(typeof(NSObject))]
    interface MSACWrapperExceptionManager
    {
        //+ (void) saveWrapperException:(MSACWrapperException*) wrapperException;
        [Static]
        [Export("saveWrapperException:")]
        void SaveWrapperException(MSACWrapperException wrapperException);

        //+ (MSACWrapperException*) loadWrapperExceptionWithUUID:(NSString*) uuid;
        [Static]
        [Export("loadWrapperExceptionWithUUIDString:")]
        MSACWrapperException LoadWrapperExceptionWithUUID(string uuidString);
    }

    [BaseType(typeof(NSObject))]
    interface MSACWrapperException
    {
        //@property(nonatomic, strong) MSACException* exception;
        [Export("modelException")]
        MSACWrapperExceptionModel Exception { get; set; }

        //@property(nonatomic, strong) NSData* exceptionData;
        [Export("exceptionData")]
        NSData ExceptionData { get; set; }

        [Export("processId")]
        NSNumber ProcessId { get; set; }
    }

    [BaseType(typeof(NSObject))]
    interface MSACWrapperCrashesHelper
    {
        //+ (void) setCrashHandlerSetupDelegate:(id<MSACCrashHandlerSetupDelegate>) delegate;
        [Static]
        [Export("setCrashHandlerSetupDelegate:")]
        void SetCrashHandlerSetupDelegate(MSACCrashHandlerSetupDelegate del);
    }
}
