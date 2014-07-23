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
using VirtualGuide.Mobile.Common;

namespace VirtualGuide.Mobile.ViewModel
{
    public class TravelViewModel
    {
        public ObservableCollection<Travel> Travels { get; private set; }

        public async Task<List<Travel>> LoadAvailableTravels()
        {
            return await HttpHelper.GetData<List<Travel>>("api/Travels");
        }

        public async Task<List<Travel>> LoadOwnedTravels()
        {
            return await HttpHelper.GetData<List<Travel>>("api/OwnedTravels");
        }

        public async Task AddItemsToDb(List<Travel> travels)
        {
            await App.Connection.InsertOrReplaceAllAsync(travels);
        }

        public async Task<List<Travel>> GetItemsFromDb()
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel");

            return travels;
        }
        
        public async Task DownloadAndSaveOwnedTravels()
        {
            var travels = await LoadAvailableTravels();
            await AddItemsToDb(travels);

            //Download properties
            foreach (var travel in travels)
            {

            }
        }
    }
}
