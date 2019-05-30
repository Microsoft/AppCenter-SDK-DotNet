// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Windows.Forms;

namespace Contoso.WinForms.Demo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.SetLogUrl("https://in.appcenter.ms");
            AppCenter.Start("42f4a839-c54c-44da-8072-a2f2a61751b2", typeof(Analytics), typeof(Crashes));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
