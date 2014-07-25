using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.ViewModel;

namespace VirtualGuide.Mobile.Repository
{
    class PlaceRepository
    {
        private async Task<List<Place>> GetParentPlacesByTravelIdAsync(int TravelId)
        {
            var places = await App.Connection.QueryAsync<Place>("Select * FROM Place WHERE TravelId = ? AND (ParentId IS NULL OR ParentId = '' ) ORDER BY Latitude DESC", TravelId);

            return places;
        }

        public async Task<List<MapPlaceViewModel>> GetSimplePlaces(int travelId)
        {
            var places = await GetParentPlacesByTravelIdAsync(travelId);

            return ModelHelper.ObjectToViewModel<MapPlaceViewModel, Place>(places);
        }
    }
}
