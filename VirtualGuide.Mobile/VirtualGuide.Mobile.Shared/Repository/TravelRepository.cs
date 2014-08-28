using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.ViewModel;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.Repository
{
    public class TravelRepository
    {
        #region access webservice

        private async Task<List<Travel>> LoadAvailableTravels()
        {
            return await HttpHelper.GetData<List<Travel>>("api/Travels");
        }

        private async Task<List<Travel>> LoadOwnedTravels()
        {
            return await HttpHelper.GetData<List<Travel>>("api/OwnedTravels");
        }

        public async Task<List<TravelViewModel>> GetAvailableTravels()
        {
            var availableTravels = await LoadAvailableTravels();
            var viewModels = ModelHelper.ObjectToViewModel<TravelViewModel, Travel>(availableTravels);

            foreach(var model in viewModels)
            {
                model.IsOwned = false;
            }

            return viewModels;
        }

        #endregion

        #region access database

        public async Task<List<TravelViewModel>> GetAllTravelsAsync()
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel");
            var viewModels = ModelHelper.ObjectToViewModel<TravelViewModel, Travel>(travels);
            return viewModels;
        }

        public async Task<TravelViewModel> GetTravelByIdAsync(int id)
        {
            var travel = await App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE Id=?", id);

            if (travel.Count != 1)
            {
                throw new Exception("Entity not found");
            }

            return new TravelViewModel(travel[0]);
        }

        private async Task<List<Travel>> GetOwnedTravels()
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE IsOwned=?", true);

            return travels;
        }

        public async Task<List<TravelViewModel>> DownloadAndSaveAllTravels()
        {
            var ownedTravelsTask = await DownloadAndSaveOwnedTravels();
            var availableTravelsTask = await DownloadAndSaveAvailableTravels(ownedTravelsTask);

            var allTravels = new List<TravelViewModel>();
            allTravels.AddRange(ownedTravelsTask);
            allTravels.AddRange(availableTravelsTask);

            return allTravels;
        }

        private async Task<List<TravelViewModel>> DownloadAndSaveOwnedTravels()
        {
            var travels = await LoadOwnedTravels();
            foreach (var travel in travels) travel.IsOwned = true;
            await App.Connection.InsertOrReplaceAllAsync(travels);

            
            foreach (var travel in travels)
            {
                await App.Connection.InsertOrReplaceAllAsync(travel.Properties);
                await App.Connection.InsertOrReplaceAllAsync(travel.Places);
            }
            
            DownloadMedia(travels);

            var viewModels = ModelHelper.ObjectToViewModel<TravelViewModel, Travel>(travels);
            
            return viewModels;
        }

        private async Task<List<TravelViewModel>> DownloadAndSaveAvailableTravels(List<TravelViewModel> ownedTravels)
        {
            var travels = await LoadAvailableTravels();
            var newTravels = new List<Travel>();
            foreach (var travel in travels)
            {
                if (!ownedTravels.Exists(x => x.Id == travel.Id))
                {
                    newTravels.Add(travel);
                    travel.IsOwned = false;
                }
            }

            await App.Connection.InsertOrReplaceAllAsync(newTravels);

            DownloadMedia(newTravels);

            var viewModels = ModelHelper.ObjectToViewModel<TravelViewModel, Travel>(newTravels);

            return viewModels;
        }


        private async void DownloadMedia(List<Travel> travels)
        {
            await HttpHelper.ImageDownloader<Travel>(travels);
        }

        #endregion

    }
}
