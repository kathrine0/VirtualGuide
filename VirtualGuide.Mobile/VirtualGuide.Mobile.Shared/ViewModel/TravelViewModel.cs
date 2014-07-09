using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtualGuide.Mobile.Model;
using System.Net.Http;

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
        public async void LoadData()
        {
            if (this.IsDataLoaded == false)
            {
                HttpClient client = new HttpClient();
                //try
                //{
                    string responseBody = await client.GetStringAsync(App.WebService + "Travels");
                    Parse(responseBody);

                //}
                //catch (HttpRequestException e)
                //{
                    
                //}

                client.Dispose();
            }
        }

        private void Parse(string responseBody)
        {
            //try
            //{
            //    this.Travels.Clear();
            //    if (e.Result != null)
            //    {
            //        var items = JsonConvert.DeserializeObject<BookDetails[]>(e.Result);
            //        int id = 0;
            //        foreach (Travel item in items)
            //        {
            //            this.Items.Add(new ItemViewModel()
            //            {
            //                ID = (id++).ToString(),
            //                LineOne = book.Title,
            //                LineTwo = book.Author,
            //                LineThree = book.Description.Replace("\n", " ")
            //            });
            //        }
            //        this.IsDataLoaded = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.Items.Add(new ItemViewModel()
            //    {
            //        ID = "0",
            //        LineOne = "An Error Occurred",
            //        LineTwo = String.Format("The following exception occured: {0}", ex.Message),
            //        LineThree = String.Format("Additional inner exception information: {0}", ex.InnerException.Message)
            //    });
            //}
        }
    }
}
