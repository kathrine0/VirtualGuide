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
                HttpClient client = new HttpClient();
                try
                {
                    var response = await client.PostAsync(String.Format("{0}Token", App.WebService), new StringContent(String.Format("grant_type=password&username={0}&password={1}", username, password)));

                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    //throw exception if status 400 or no connection
                    //TODO: different error when invalid login, different when no connection

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        string responseBody = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

                        if (!result.ContainsKey("access_token")) throw new Exception();

                        settingsDataHelper.SetValue(SettingsDataHelper.TOKEN, result["access_token"]);
                    }
                    else
                    {
                        throw new HttpRequestException(response.StatusCode.ToString());
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

        //public async Task Logout()
        //{
            
        //    HttpClient client = new HttpClient();
        //    try
        //    {
        //        var response = await client.PostAsync();

        //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        //throw exception if status 400 or no connection
        //        //TODO: different error when invalid login, different when no connection

        //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //        {

        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

        //            if (!result.ContainsKey("access_token")) throw new Exception();

        //            settingsDataHelper.SetValue(SettingsDataHelper.TOKEN, result["access_token"]);
        //        }
        //        else
        //        {
        //            throw new HttpRequestException(response.StatusCode.ToString());
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        client.Dispose();
        //    }
            
        //}

        //public async Task Register()
        //{

        //}
    }
}
