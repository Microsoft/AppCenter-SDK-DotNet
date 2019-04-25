﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Foundation;
using Microsoft.AppCenter.iOS.Bindings;

namespace Microsoft.AppCenter.Auth.iOS.Bindings
{
    // @interface MSIdentity : MSService
    [BaseType(typeof(NSObject))]
    interface MSAuth
    {
        // + (void)setConfigUrl:(NSString*) configUrl;
        [Static]
        [Export("setConfigUrl:")]
        void SetConfigUrl(string configUrl);

        // + (BOOL)isEnabled;
        [Static]
        [Export("isEnabled")]
        bool IsEnabled();

        // + (void)setEnabled:(BOOL)isEnabled;
        [Static]
        [Export("setEnabled:")]
        void SetEnabled(bool isEnabled);

        // + (void)signInWithCompletionHandler:(MSSignInCompletionHandler _Nullable) completionHandler;
        [Static]
        [Export("signInWithCompletionHandler:")]
        void SignIn(MSSignInCompletionHandler completionHandler);

        // + (void)signOut;
        [Static]
        [Export("signOut")]
        void SignOut();

        // + (void)openURL:(NSURL *)url;
        [Static]
        [Export("openURL: options:")]
        void OpenUrl(NSUrl url,NSDictionary options);
    }

    // typedef void (^MSSignInCompletionHandler)(MSUserInformation* _Nullable userInformation, NSError * _Nullable error);
    delegate void MSSignInCompletionHandler([NullAllowed] MSUserInformation userInformation, [NullAllowed] NSError error);
}
