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
        public async Task Login(string username, string password)
        {
            if (!String.IsNullOrEmpty(username) && !(String.IsNullOrEmpty(password)))
            {
                var credentials = String.Format("grant_type=password&username={0}&password={1}", username, password);

                var result = await HttpHelper.PostData<Dictionary<string, string>>("Token", credentials);

                if (!result.ContainsKey("access_token")) throw new Exception();

                settingsDataHelper.SetValue(SettingsDataHelper.TOKEN, result["access_token"]);         
            }
        }

        public async Task Logout()
        {
            await HttpHelper.PostData<object>("api/Account/Logout", null);

            settingsDataHelper.RemoveValue(SettingsDataHelper.TOKEN);
        }
    }
}
