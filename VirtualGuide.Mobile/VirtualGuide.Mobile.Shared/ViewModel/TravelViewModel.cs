using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtualGuide.Mobile.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace VirtualGuide.Mobile.ViewModel
{
    class TravelViewModel
    {
        public ObservableCollection<Travel> Travels { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async Task<List<Travel>> LoadData()
        {
            if (this.IsDataLoaded == false)
            {
                HttpClient client = new HttpClient();
                //try
                //{
                    string responseBody = await client.GetStringAsync(App.WebService + "Travels");
                    var result = Parse(responseBody);

                //}
                //catch (HttpRequestException e)
                //{
                    
                //}

                client.Dispose();

                return result;
            }

            return null;
        }

        private List<Travel> Parse(string responseBody)
        {
            var travels = new List<Travel>();
            if (!String.IsNullOrEmpty(responseBody))
            {
                var result = JsonConvert.DeserializeObject<List<Travel>>(responseBody);

                return result;
            }

            return travels;
        }
    }
}
