using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Helper;

namespace VirtualGuide.Mobile.Repository
{
    public class UserRepository
    {
        SettingsDataHelper settingsDataHelper = new SettingsDataHelper();
        public async Task<bool> Login(string email, string password)
        {
            if (!String.IsNullOrEmpty(email) && !(String.IsNullOrEmpty(password)))
            {
                var credentials = String.Format("grant_type=password&username={0}&password={1}", email, password);

                var result = await HttpHelper.PostData<Dictionary<string, string>>("Token", credentials);

                if (!result.ContainsKey("access_token")) throw new Exception();

                settingsDataHelper.SetValue(SettingsDataHelper.TOKEN, result["access_token"]);
                return true;
            }

            return false;
        }

        public async Task Logout()
        {
            await HttpHelper.PostData<object>("api/Account/Logout", null);

            settingsDataHelper.RemoveValue(SettingsDataHelper.TOKEN);
        }

        public async Task Register(string username, string password, string repeatPassword)
        {
            var data = new
            {
                Email = username,
                Password = password,
                ConfirmPassword = repeatPassword
            };

            var result = await HttpHelper.PostData<object>("api/Account/Register", data);
        }
    }
}
