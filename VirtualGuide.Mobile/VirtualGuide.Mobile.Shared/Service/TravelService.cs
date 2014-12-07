using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.Repository;

namespace VirtualGuide.Mobile.Service
{
    class TravelService
    {
        private SettingsDataHelper settingsDataHelper = new SettingsDataHelper();
        private TravelRepository _travelRepository = new TravelRepository();

        #region access webservice

        public async Task<List<GuideListBindingModel>> DownloadAvailableTravels()
        {
            var travels = await HttpHelper.GetData<List<Travel>>("api/Travels");
            HttpHelper.ImageDownloader<Travel>(travels);
            foreach (var travel in travels) travel.IsOwned = false;

            _travelRepository.SaveAvailableTravels(travels);

            return AutoMapper.Mapper.Map<List<GuideListBindingModel>>(travels);
        }

        public async Task<List<GuideListBindingModel>> DownloadOwnedTravels()
        {
            if (!String.IsNullOrEmpty(settingsDataHelper.GetValue<string>(SettingsDataHelper.TOKEN)))
            {

                var travels = await HttpHelper.GetData<List<Travel>>("api/OwnedTravels");
                HttpHelper.ImageDownloader<Travel>(travels);
                foreach (var travel in travels) travel.IsOwned = true;

                this.SaveOwnedTravelsAndDownloadImages(travels);

                return AutoMapper.Mapper.Map<List<GuideListBindingModel>>(travels);
            }

            //if user is not authenticated
            return new List<GuideListBindingModel>();
        }

        public async Task<GuideListBindingModel> DownloadBoughtTravel(int id)
        {
            try
            {
                var t = await _travelRepository.GetTravelById(id);
                if (t.IsOwned)
                    throw new TravelAlreadyOwnedException();
            }
            catch (EntityNotFoundException)
            {
                //It's OK if the entity wasn't found. Continue with downloading it
            }

            Travel travel;

            if (UserService.IsUserLoggedIn())
                travel = await HttpHelper.PostData<Travel>(String.Format("api/BuyTravel/{0}", id));
            else
                travel = await HttpHelper.PostData<Travel>(String.Format("api/BuyTravelAnonymous/{0}", id));

            travel.IsOwned = true;

            var travelList = new List<Travel>() { travel };
            HttpHelper.ImageDownloader<Travel>(travelList);

            this.SaveOwnedTravelsAndDownloadImages(travelList);

            return AutoMapper.Mapper.Map<GuideListBindingModel>(travel);
        }

        public async void SaveOwnedTravelsAndDownloadImages(List<Travel> travels)
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

        #endregion
    }
}
