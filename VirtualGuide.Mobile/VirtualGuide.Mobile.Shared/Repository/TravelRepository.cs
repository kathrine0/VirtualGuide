﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;

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

            return AutoMapper.Mapper.Map<List<GuideListBindingModel>>(travels);
        }

        public async Task<List<GuideListBindingModel>> DownloadOwnedTravels()
        {
            if (!String.IsNullOrEmpty(settingsDataHelper.GetValue<string>(SettingsDataHelper.TOKEN)))
            {

                var travels = await HttpHelper.GetData<List<Travel>>("api/OwnedTravels");
                HttpHelper.ImageDownloader<Travel>(travels);
                foreach (var travel in travels) travel.IsOwned = true;

                SaveOwnedTravelsAndDownloadImages(travels);

                return AutoMapper.Mapper.Map<List<GuideListBindingModel>>(travels);
            }

            //if user is not authenticated
            return new List<GuideListBindingModel>();
        }
       
        public async Task<GuideListBindingModel> DownloadBoughtTravel(int id)
        {
            try
            {
                var t = await this.GetTravelById(id);
                if (t.IsOwned)
                    throw new TravelAlreadyOwnedException();
            } catch (EntityNotFoundException)
            {
                //It's OK if the entity wasn't found. Continue with downloading it
            }

            Travel travel;

            if (UserRepository.IsUserLoggedIn())
                travel = await HttpHelper.PostData<Travel>(String.Format("api/BuyTravel/{0}", id));
            else
                travel = await HttpHelper.PostData<Travel>(String.Format("api/BuyTravelAnonymous/{0}", id));

            travel.IsOwned = true;
            
            var travelList = new List<Travel>() { travel };
            HttpHelper.ImageDownloader<Travel>(travelList);

            SaveOwnedTravelsAndDownloadImages(travelList);

            return AutoMapper.Mapper.Map<GuideListBindingModel>(travel);
        }

        #endregion

        #region access database

        #region public methods
        public async Task<List<GuideListBindingModel>> GetAllTravelsAsync() 
        {
            var travels = await App.Connection.QueryAsync<Travel>("Select * FROM Travel");

            return AutoMapper.Mapper.Map<List<GuideListBindingModel>>(travels);
        }
        public async Task<T> GetTravelByIdAsync<T>(int id) 
            where T : BaseTravelBindingModel
        {
            var travel = await this.GetTravelById(id);

            return AutoMapper.Mapper.Map<T>(travel);
        }

        #endregion

        #region private methods
        private async Task<List<Travel>> GetOwnedTravels()
        {
            var query = App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE IsOwned=?", true);
            var travels = await query.ConfigureAwait(false);

            return travels;
        }

        private async Task<Travel> GetTravelById(int id)
        {
            var query = App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE Id=?", id);
            var travel = await query.ConfigureAwait(false);

            if (travel.Count != 1)
            {
                throw new EntityNotFoundException();
            }

            return travel[0];
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
            await App.Connection.InsertOrIgnoreAllAsync(travels).ConfigureAwait(false);
        }

        #endregion


        #endregion

    }
}
