using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.ViewModel;

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

        public async Task<List<TravelViewModel>> DownloadAndSaveOwnedTravels()
        {
            var travels = await LoadOwnedTravels();
            await App.Connection.InsertOrReplaceAllAsync(travels);

            foreach (var travel in travels)
            {
                await App.Connection.InsertOrReplaceAllAsync(travel.Properties);
            }

            var viewModels = ModelHelper.ObjectToViewModel<TravelViewModel, Travel>(travels);
            return viewModels;
        }

        #endregion

    }
}
