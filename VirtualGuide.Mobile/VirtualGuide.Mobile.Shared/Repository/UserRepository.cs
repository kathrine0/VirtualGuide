using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Mobile.Repository
{
    public class UserRepository
    {
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
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

                    if (!result.ContainsKey("access_token")) throw new Exception();

                    App.AuthToken = result["access_token"];
                }
                catch (HttpRequestException)
                {
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
        }
    }
}
