using System;
using System.Collections.Generic;
using System.Linq;
using VirtualGuide.Mobile.BindingModel.User;
using Windows.Storage;

namespace VirtualGuide.Mobile.Helper
{
 
    /// <summary>
    /// This class is responsible for storing runtime data. The data are
    /// removed when the app is terminated
    /// </summary>
    public class LocalDataHelper : BaseDataHelper
    {
        protected override string CONTAINER_NAME 
        {
            get {
                return "runtimeData";
            }
        }

        public const string LOCATION_MSG = "locationMessage";
        public const string REFRESH_NOW = "refreshNow";

    }

    /// <summary>
    /// This class is responsible for storing settings data. 
    /// Data are saved in the memory of the app
    /// </summary>
    public class SettingsDataHelper : BaseDataHelper
    {
        protected override string CONTAINER_NAME
        {
            get
            {
                return "settings";
            }
        }

        /// <summary>
        /// Settings Keys
        /// </summary>
        public const string LAST_LOGGED_USERS = "lastLoggedUsers";
        public const string TOKEN = "token";
    }

    public class UsersDataHelper
    {
        SettingsDataHelper sdh = new SettingsDataHelper();

        public DateTime? GetLastUserLogin(string username)
        {
            var collection = sdh.GetValue<IList<LastLoggedUserSettingsModel>>(SettingsDataHelper.LAST_LOGGED_USERS);

            var item = collection.FirstOrDefault(x => x.Username == username);

            if (item != null)
            {
                return item.LastLogin;
            }
            else
            {
                return null;
            }
        }

        public void SetLastUserLogin(string username)
        {
            var lastLoggedUserSettingModel = new LastLoggedUserSettingsModel()
            {
                LastLogin = DateTime.Now,
                Username = username,
            };

            sdh.AddToValue<LastLoggedUserSettingsModel>(SettingsDataHelper.LAST_LOGGED_USERS, lastLoggedUserSettingModel);

        }
    }
    
    public abstract class BaseDataHelper
    {
        private static ApplicationDataContainer _appSettings = ApplicationData.Current.LocalSettings;
        protected abstract string CONTAINER_NAME { get;  }

        public void CreateContainer()
        {
            ApplicationDataContainer container = _appSettings.CreateContainer(CONTAINER_NAME, Windows.Storage.ApplicationDataCreateDisposition.Always);
        }

        public void DeleteContainer()
        {
            if (_appSettings.Containers.ContainsKey(CONTAINER_NAME))
            {
                _appSettings.DeleteContainer(CONTAINER_NAME);
            }
        }

        public bool KeyExists(string key)
        {
            ContainerExists();

            if (_appSettings.Containers[CONTAINER_NAME].Values.ContainsKey(key))
            {
                return true;
            }

            return false;
        }

        public T GetValue<T>(string key)
        {
            if (!KeyExists(key)) return default(T);

            return (T) _appSettings.Containers[CONTAINER_NAME].Values[key];
        }

        public void SetValue(string key, object value)
        {
            ContainerExists();

            _appSettings.Containers[CONTAINER_NAME].Values[key] = value;
        }

        public void RemoveValue(string key)
        {
            ContainerExists();

            _appSettings.Containers[CONTAINER_NAME].Values.Remove(key);
        }

        public void AddToValue<T>(string key, T value)
        {
            ContainerExists();

            IList<T> collection = null;

            if (KeyExists(key))
            {
                collection = GetValue<IList<T>>(key);
            }
            else
            {
                collection = new List<T>();
            }

            collection.Add(value);
            SetValue(key, collection);
        }

        private void ContainerExists()
        {
            if (!_appSettings.Containers.ContainsKey(CONTAINER_NAME))
            {
                CreateContainer();
            }
        }
        
    }
}
