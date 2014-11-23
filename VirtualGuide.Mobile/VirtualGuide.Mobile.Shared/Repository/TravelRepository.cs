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

        public async Task<List<GuideListBindingModel>> DownloadAvailableTravels()
        {
            var travels = await HttpHelper.GetData<List<Travel>>("api/Travels");
            HttpHelper.ImageDownloader<Travel>(travels);
            foreach (var travel in travels) travel.IsOwned = false;

            SaveAvailableTravels(travels);

            return ModelHelper.ObjectToViewModel<GuideListBindingModel, Travel>(travels);
        }

        public async Task<List<GuideListBindingModel>> DownloadOwnedTravels()
        {
            if (!String.IsNullOrEmpty(settingsDataHelper.GetValue<string>(SettingsDataHelper.TOKEN)))
            {

                var travels = await HttpHelper.GetData<List<Travel>>("api/OwnedTravels");
                HttpHelper.ImageDownloader<Travel>(travels);
                foreach (var travel in travels) travel.IsOwned = true;

                SaveOwnedTravelsAndDownloadImages(travels);

                return ModelHelper.ObjectToViewModel<GuideListBindingModel, Travel>(travels);
            }

            //if user is not authenticated
            return new List<GuideListBindingModel>();
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
            var query = App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE Id=?", id);
            var travel = await query.ConfigureAwait(false);

            if (travel.Count != 1)
            {
                throw new Exception("Entity not found");
            }

            return (T) Activator.CreateInstance(typeof(T), travel[0]);
        }

        #endregion

        #region private methods
        private async Task<List<Travel>> GetOwnedTravels()
        {
            var query = App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE IsOwned=?", true);
            var travels = await query.ConfigureAwait(false);

            return travels;
        }

        private async void SaveOwnedTravelsAndDownloadImages(List<Travel> travels)
        {
            var downloadTask = new List<Task>();
            downloadTask.Add(HttpHelper.MapDownloader(travels));

            await App.Connection.InsertOrReplaceAllAsync(travels).ConfigureAwait(false);

            foreach (var travel in travels)
            {
                await App.Connection.InsertOrReplaceAllAsync(travel.Properties).ConfigureAwait(false);
                await App.Connection.InsertOrReplaceAllAsync(travel.Places).ConfigureAwait(false);
                downloadTask.Add(HttpHelper.ImageDownloaderAsync<Place>(travel.Places));
            }

            //callback!
            await Task.WhenAll(downloadTask);
        }

        private async void SaveAvailableTravels(List<Travel> travels)
        {
            await App.Connection.InsertOrReplaceAllAsync(travels).ConfigureAwait(false);
        }

        #endregion


        #endregion

    }
}
