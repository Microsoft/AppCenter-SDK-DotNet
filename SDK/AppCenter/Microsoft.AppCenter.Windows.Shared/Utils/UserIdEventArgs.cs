﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AppCenter.Windows.Shared.Utils
{
    /// <summary>
    /// Event args for event that occurs when an user change is received.
    /// </summary>
    public class UserIdUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// User Id
        /// </summary>
        public string UserId;
    }
}