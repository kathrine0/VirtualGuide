using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Mobile.Helper
{
    public class HttpHelper
    {
        public static async Task<T> GetData<T>(string webPath)
        {
            HttpClient client = new HttpClient();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            try
            {
                //authenticate request
                if (localSettings.Values["token"] != null)
                {
                    var token = (string) localSettings.Values["token"];
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.GetAsync(App.WebService + webPath);


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = ParseData<T>(responseBody);
                    return result;
                }
                else
                {
                    throw new HttpRequestException(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Dispose();
            }

        }

        private static T ParseData<T>(string responseBody)
        {
            if (!String.IsNullOrEmpty(responseBody))
            {
                var result = JsonConvert.DeserializeObject<T>(responseBody);

                return result;
            }

            throw new ArgumentNullException("Response Body is null or empty");
        }
    }
}
