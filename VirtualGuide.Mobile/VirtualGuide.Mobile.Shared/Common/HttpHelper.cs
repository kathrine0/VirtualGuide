using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Mobile.Common
{
    public class HttpHelper
    {
        public static async Task<T> GetData<T>(string webPath)
        {
            HttpClient client = new HttpClient();
            try
            {
                //authenticate request
                if (!String.IsNullOrEmpty(App.AuthToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
                }

                string responseBody = await client.GetStringAsync(App.WebService + webPath);
                var result = ParseData<T>(responseBody);
                return result;
            }
            catch (HttpRequestException e)
            {
                //TODO: Handle no internet connection
                throw;
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
