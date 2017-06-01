﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.Azure.Mobile.Utils
{
    public class ApplicationSettings : IApplicationSettings
    {
        private readonly object configLock = new object();
        private IDictionary<string, string> current;

        public ApplicationSettings()
        {
            current = ReadAll();
        }

        public object this[string key]
        {
            get
            {
                lock (configLock)
                {
                    string value = null;
                    current.TryGetValue(key, out value);
                    return value;
                }
            }
            set
            {
                var invariant = value != null ? TypeDescriptor.GetConverter(value.GetType()).ConvertToInvariantString(value) : null;
                lock (configLock)
                {
                    current[key] = invariant;
                    SaveValue(key, invariant);
                }
            }
        }

        public void Remove(string key)
        {
            lock (configLock)
            {
                current.Remove(key);
                var config = OpenConfiguration();
                config.AppSettings.Settings.Remove(key);
                config.Save();
            }
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            lock (configLock)
            {
                if (current.ContainsKey(key))
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(current[key]);
                }
            }
            this[key] = defaultValue;
            return defaultValue;
        }

        private IDictionary<string, string> ReadAll()
        {
            var config = OpenConfiguration();
           return config.AppSettings.Settings.Cast<KeyValueConfigurationElement>().ToDictionary(e => e.Key, e => e.Value);
        }

        private void SaveValue(string key, string value)
        {
            lock (configLock)
            {
                var config = OpenConfiguration();
                var element = config.AppSettings.Settings[key];
                if (element == null)
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                else
                {
                    element.Value = value;
                }
                config.Save();
            }
        }

        private static Configuration OpenConfiguration()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var path = Path.Combine(Path.GetDirectoryName(location), "MobileCenter.config");
            var executionFileMap = new ExeConfigurationFileMap { ExeConfigFilename = path };
            return ConfigurationManager.OpenMappedExeConfiguration(executionFileMap, ConfigurationUserLevel.None);
        }
    }
}
