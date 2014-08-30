using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace VirtualGuide.Mobile.Helper
{
    public class LocalDataHelper
    {
        private static ApplicationDataContainer _appSettings = ApplicationData.Current.LocalSettings;
        private const string CONTAINER_NAME = "runtimeData";

        public static void CreateContainer()
        {
            ApplicationDataContainer container = _appSettings.CreateContainer(CONTAINER_NAME, Windows.Storage.ApplicationDataCreateDisposition.Always);
        }

        public static void DeleteContainer()
        {
            if (_appSettings.Containers.ContainsKey(CONTAINER_NAME))
            {
                _appSettings.DeleteContainer(CONTAINER_NAME);
            }
        }

        public static bool KeyExists(string key)
        {
            ContainerExists();

            if (_appSettings.Containers[CONTAINER_NAME].Values.ContainsKey(key))
            {
                return true;
            }

            return false;
        }

        public static T GetKeyValue<T>(string key)
        {
            if (!KeyExists(key)) return default(T);

            return (T) _appSettings.Containers[CONTAINER_NAME].Values[key];
        }

        public static void SetValue(string key, object value)
        {
            ContainerExists();

            _appSettings.Containers[CONTAINER_NAME].Values[key] = value;
        }

        private static void ContainerExists()
        {
            if (!_appSettings.Containers.ContainsKey(CONTAINER_NAME))
            {
                CreateContainer();
            }
        }
        
    }
}
