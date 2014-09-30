using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
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
        private SettingsDataHelper settingsDataHelper = new SettingsDataHelper();

        #region access webservice

        private async Task<List<Travel>> LoadAvailableTravels()
        {
            return await HttpHelper.GetData<List<Travel>>("api/Travels");
        }

        private async Task<List<Travel>> LoadOwnedTravels()
        {
            return await HttpHelper.GetData<List<Travel>>("api/OwnedTravels");
        }

        public async Task<List<GuideMainBindingModel>> GetAvailableTravels()
        {
            var availableTravels = await LoadAvailableTravels();
            var viewModels = ModelHelper.ObjectToViewModel<GuideMainBindingModel, Travel>(availableTravels);

            foreach(var model in viewModels)
            {
                model.IsOwned = false;
            }

            return viewModels;
        }

        #endregion

        #region access database

        #region public methods
        public async Task<List<GuideListBindingModel>> GetAllTravelsAsync() 
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel");
            var viewModels = ModelHelper.ObjectToViewModel<GuideListBindingModel, Travel>(travels);
            return viewModels;
        }
        public async Task<T> GetTravelByIdAsync<T>(int id) 
            where T : BaseTravelBindingModel
        {
            var travel = await App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE Id=?", id);

            if (travel.Count != 1)
            {
                throw new Exception("Entity not found");
            }

            return (T) Activator.CreateInstance(typeof(T), travel[0]);
        }
        public async Task<List<T>> DownloadAndSaveAllTravels<T>()
            where T : BaseTravelBindingModel
        {
            var allTravels = new List<T>();
            List<T> ownedTravels = new List<T>();

            //download owned travels, when user is logged in
            if (!String.IsNullOrEmpty(settingsDataHelper.GetValue<string>(SettingsDataHelper.TOKEN)))
            {
                ownedTravels = await DownloadAndSaveOwnedTravels<T>();
                allTravels.AddRange(ownedTravels);
            }
            var availableTravels = await DownloadAndSaveAvailableTravels<T>(ownedTravels);

            allTravels.AddRange(availableTravels);

            return allTravels;
        }

        #endregion

        #region private methods
        private async Task<List<Travel>> GetOwnedTravels()
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE IsOwned=?", true);

            return travels;
        }
        private async Task<List<T>> DownloadAndSaveOwnedTravels<T>() 
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

            var viewModels = ModelHelper.ObjectToViewModel<T, Travel>(travels);
            
            return viewModels;
        }
        private async Task<List<T>> DownloadAndSaveAvailableTravels<T>(List<T> ownedTravels)
            where T : BaseTravelBindingModel
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

            var viewModels = ModelHelper.ObjectToViewModel<T, Travel>(newTravels);

            return viewModels;
        }

        #endregion


        #endregion

    }
}
