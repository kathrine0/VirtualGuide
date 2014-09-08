using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.DBModel;
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

        public async Task<List<TravelModel>> GetAvailableTravels()
        {
            var availableTravels = await LoadAvailableTravels();
            var viewModels = ModelHelper.ObjectToViewModel<TravelModel, Travel>(availableTravels);

            foreach(var model in viewModels)
            {
                model.IsOwned = false;
            }

            return viewModels;
        }

        #endregion

        #region access database

        #region public methods
        public async Task<List<TravelModel>> GetAllTravelsAsync() 
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel");
            var viewModels = ModelHelper.ObjectToViewModel<TravelModel, Travel>(travels);
            return viewModels;
        }
        public async Task<TravelModel> GetTravelByIdAsync(int id) 
        {
            var travel = await App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE Id=?", id);

            if (travel.Count != 1)
            {
                throw new Exception("Entity not found");
            }

            return new TravelModel(travel[0]); //(T)Activator.CreateInstance(typeof(T), travel[0]);
        }
        public async Task<List<TravelModel>> DownloadAndSaveAllTravels() 
        {
            var ownedTravelsTask = await DownloadAndSaveOwnedTravels();
            var availableTravelsTask = await DownloadAndSaveAvailableTravels(ownedTravelsTask);

            var allTravels = new List<TravelModel>();
            allTravels.AddRange(ownedTravelsTask);
            allTravels.AddRange(availableTravelsTask);

            return allTravels;
        }

        #endregion

        #region private methods
        private async Task<List<Travel>> GetOwnedTravels()
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE IsOwned=?", true);

            return travels;
        }
        private async Task<List<TravelModel>> DownloadAndSaveOwnedTravels() 
        {
            var travels = await LoadOwnedTravels();
            var downloadTask = new List<Task>();
            
            foreach (var travel in travels) travel.IsOwned = true;
            await App.Connection.InsertOrReplaceAllAsync(travels);
            
            downloadTask.Add(HttpHelper.MapDownloader(travels));
            downloadTask.Add(HttpHelper.ImageDownloader<Travel>(travels));
            foreach (var travel in travels)
            {
                await App.Connection.InsertOrReplaceAllAsync(travel.Properties);

                await App.Connection.InsertOrReplaceAllAsync(travel.Places);
                downloadTask.Add(HttpHelper.ImageDownloader<Place>(travel.Places));
            }

            await Task.WhenAll(downloadTask);

            var viewModels = ModelHelper.ObjectToViewModel<TravelModel, Travel>(travels);
            
            return viewModels;
        }
        private async Task<List<TravelModel>> DownloadAndSaveAvailableTravels(List<TravelModel> ownedTravels)
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

            await HttpHelper.ImageDownloader<Travel>(newTravels);

            var viewModels = ModelHelper.ObjectToViewModel<TravelModel, Travel>(newTravels);

            return viewModels;
        }

        #endregion


        #endregion

    }
}
