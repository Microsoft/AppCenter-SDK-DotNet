﻿using System;
using System.Drawing;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Azure.Mobile.Utils
{
    /// <summary>
    /// Implements the abstract device information helper class
    /// </summary>
    public class DeviceInformationHelper : AbstractDeviceInformationHelper
    {
        public static event EventHandler InformationInvalidated;

        protected override string GetSdkName()
        {
            return "mobilecenter.windowsclassic";
        }

        protected override string GetDeviceModel()
        {
            var managementClass = new ManagementClass("Win32_ComputerSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                return (string)managementObject["Model"];
            }
            return string.Empty;
        }

        protected override string GetAppNamespace()
        {
             return Assembly.GetEntryAssembly().EntryPoint.DeclaringType.Namespace;
        }

        protected override string GetDeviceOemName()
        {
            var managementClass = new ManagementClass("Win32_ComputerSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                return (string)managementObject["Manufacturer"];
            }
            return string.Empty;
        }

        protected override string GetOsName()
        {
            var managementClass = new ManagementClass("Win32_OperatingSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                return (string)managementObject["Caption"];
            }
            return string.Empty;
        }

        protected override string GetOsBuild()
        {
            var managementClass = new ManagementClass("Win32_OperatingSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                return $"{(string)managementObject["Version"]}.{Environment.OSVersion.Version.Revision}";
            }
            return string.Empty;
        }

        protected override string GetOsVersion()
        {
            var managementClass = new ManagementClass("Win32_OperatingSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                return (string)managementObject["Version"];
            }
            return string.Empty;
        }

        protected override string GetAppVersion()
        {
            return Application.ProductVersion;
        }

        protected override string GetAppBuild()
        {
            return Application.ProductVersion;
        }

        protected override string GetScreenSize()
        {          
            const int DESKTOPVERTRES = 117;
            const int DESKTOPHORZRES = 118;
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                var desktop = graphics.GetHdc();
                var height = GetDeviceCaps(desktop, DESKTOPVERTRES);
                var width = GetDeviceCaps(desktop, DESKTOPHORZRES);
                return $"{width}x{height}";
            }
        }

        /// <summary>
        /// Import GetDeviceCaps function to retreive scale-independent screen size.
        /// </summary>
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}
