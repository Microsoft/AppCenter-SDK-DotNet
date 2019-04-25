﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Microsoft.AppCenter.Auth.iOS.Bindings;

namespace Microsoft.AppCenter.Auth
{
    public partial class Auth : AppCenterService
    {
        [Preserve]
        public static Type BindingType => typeof(MSAuth);

        private static void PlatformSetConfigUrl(string configUrl)
        {
            MSAuth.SetConfigUrl(configUrl);
        }

        static Task<bool> PlatformIsEnabledAsync()
        {
            return Task.FromResult(MSAuth.IsEnabled());
        }

        static Task PlatformSetEnabledAsync(bool enabled)
        {
            MSAuth.SetEnabled(enabled);
            return Task.FromResult(default(object));
        }

        private static Task<UserInformation> PlatformSignInAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<UserInformation>();
            MSAuth.SignIn((userInformation, error) =>
            {
                if (error != null)
                {
                    try
                    {
                        throw new NSErrorException(error);
                    }
                    catch (NSErrorException e)
                    {
                        taskCompletionSource.TrySetException(e);
                    }
                }
                else
                {
                    taskCompletionSource.TrySetResult(new UserInformation
                    {
                        AccountId = userInformation.AccountId
                    });
                }
            });
            return taskCompletionSource.Task;
        }

        private static void PlatformSignOut()
        {
            MSAuth.SignOut();
        }

        /// <summary>
        /// Process URL request for the Auth service.
        /// Place this method call into app delegate openUrl method.
        /// </summary>
        /// <param name="url">The url with parameters.</param>
        public static void OpenUrl(NSUrl url,IDictionary<string,string> options)
        {
            MSAuth.OpenUrl(url,NSDictionary.FromObjectsAndKeys(options.Values.ToArray(), options.Keys.ToArray()));
        }
    }
}