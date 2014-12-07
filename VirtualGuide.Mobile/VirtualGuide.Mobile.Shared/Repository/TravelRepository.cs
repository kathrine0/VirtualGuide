using System;
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
        public async Task<List<Travel>> GetOwnedTravels()
        {
            var query = App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE IsOwned=?", true);
            var travels = await query.ConfigureAwait(false);

            return travels;
        }

        public async Task<Travel> GetTravelById(int id)
        {
            var query = App.Connection.QueryAsync<Travel>("Select * FROM Travel WHERE Id=?", id);
            var travel = await query.ConfigureAwait(false);

            if (travel.Count != 1)
            {
                throw new EntityNotFoundException();
            }

            return travel[0];
        }

        public async void SaveAvailableTravels(List<Travel> travels)
        {
            await App.Connection.InsertOrIgnoreAllAsync(travels).ConfigureAwait(false);
        }

        #endregion

    }
}
