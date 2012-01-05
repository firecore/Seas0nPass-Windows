////
//
//  Seas0nPass
//
//  Copyright 2011 FireCore, LLC. All rights reserved.
//  http://firecore.com
//
////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seas0nPass.Interfaces;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using Seas0nPass.Utils;

namespace Seas0nPass.Models
{
    public class ITunesInfoProvider : IITunesInfoProvider
    {
        private static readonly Version compatibleITunesVersion = new Version("10.5");
        public ITunesInfo CheckITunesVersion()
        {
            string iTunesPath = GetITunesExePath();
            if (string.IsNullOrEmpty(iTunesPath))
            {
                return new ITunesInfo()
                    {
                        RequiredVersion = compatibleITunesVersion.ToString(),
                        InstalledVersion = "",
                        IsCompatible = false,
                    };
            }

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(iTunesPath);
            var iTunesVersion = Version.Parse(fileVersionInfo.FileVersion);
            var iTunesInfo = new ITunesInfo()
            {
                IsCompatible = iTunesVersion >= compatibleITunesVersion,
                InstalledVersion = iTunesVersion.ToString(),
                RequiredVersion = compatibleITunesVersion.ToString()
            };
            return iTunesInfo;
        }

        private readonly string _installer11RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private readonly string _installer20RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData";
        private readonly string _localSystemUser = "S-1-5-18";
        private readonly string _products = "Products";
        private readonly string _registryPathSeparator = @"\";
        private readonly string _installProperties = "InstallProperties";
        private readonly string _displayName = "DisplayName";
        private readonly string _installLocation = "InstallLocation";
        public string GetITunesExePath()
        {
            LogUtil.LogEvent("Started iTunes exe search");
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Default;
            LogUtil.LogEvent(string.Format("Using registry view {0}", registryView));
            var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            LogUtil.LogEvent(string.Format("Opened {0} registry key", baseKey));

            // Check Installer 1.1 registry path for iTunes
            RegistryKey key11 = baseKey.OpenSubKey(_installer11RegistryPath);
            LogUtil.LogEvent(string.Format("Opened {0} registry key", key11));
            if (key11 != null)
            {
                string[] subKeyNames = key11.GetSubKeyNames();
                LogUtil.LogEvent(string.Format("Found {0} subKeyNames", subKeyNames.Length));
                foreach (string subKey in subKeyNames)
                {
                    RegistryKey installProperties = key11.OpenSubKey(subKey);
                    string iTunesExePath = GetITunesExePathInternal(installProperties);
                    if (!string.IsNullOrWhiteSpace(iTunesExePath))
                        return iTunesExePath;
                }
            }

            LogUtil.LogEvent(string.Format("iTunes was not found in {0} registry key", key11));

            // Check Installer 2.0 registry path for iTunes
            RegistryKey key20 = baseKey.OpenSubKey(
                            _installer20RegistryPath + _registryPathSeparator +
                            _localSystemUser + _registryPathSeparator +
                            _products
                        );
            LogUtil.LogEvent(string.Format("Opened {0} registry key", key20));
            if (key20 != null)
            {
                string[] subKeyNames = key20.GetSubKeyNames();
                LogUtil.LogEvent(string.Format("Found {0} subKeyNames", subKeyNames.Length));
                foreach (string subKey in subKeyNames)
                {
                    RegistryKey installProperties = key20.OpenSubKey(subKey + _registryPathSeparator + _installProperties);
                    string iTunesExePath = GetITunesExePathInternal(installProperties);
                    if (!string.IsNullOrWhiteSpace(iTunesExePath))
                        return iTunesExePath;
                }
            }

            LogUtil.LogEvent(string.Format("iTunes was not found in {0} registry key", key20));

            return "";
        }

        private string GetITunesExePathInternal(RegistryKey installProperties)
        {
            if (installProperties == null)
                return null;
             
            string name = (string)installProperties.GetValue(_displayName);
            if (name != "iTunes")
                return null;

            LogUtil.LogEvent(string.Format("Found iTunes in {0}", installProperties));
            string installPath = (string)installProperties.GetValue(_installLocation);
            if (string.IsNullOrWhiteSpace(installPath)) // skip corrupted registry entries with empty installPath
            {
                LogUtil.LogEvent(string.Format("iTunes registry entry in {0} is curruptes - InstallPath is empty", installProperties));
                return null;
            }

            string itunesExePath = Path.Combine(installPath, "iTunes.exe");
            LogUtil.LogEvent(string.Format("Path to iTunes is {0}", itunesExePath));
            return itunesExePath;
        }
    }
}
