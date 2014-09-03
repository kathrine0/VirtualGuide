using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
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

        public async Task<List<MapPlaceViewModel>> GetPlacesForMap(int travelId)
        {
            var places = await GetParentPlacesByTravelIdAsync(travelId);

            return ModelHelper.ObjectToViewModel<MapPlaceViewModel, Place>(places);
        }

        public async Task<List<ListPlaceViewModel>> GetParentPlaces(int travelId)
        {
            var places = await GetParentPlacesByTravelIdAsync(travelId);

            return ModelHelper.ObjectToViewModel<ListPlaceViewModel, Place>(places);
        }

        public async Task<PlaceViewModel> GetPlaceById(int placeId)
        {
            var place = await App.Connection.QueryAsync<Place>("Select * FROM Place WHERE Id = ?", placeId);

            if (place.Count == 0)
                throw new Exception("Element not found");

            var viewmodelList = ModelHelper.ObjectToViewModel<PlaceViewModel, Place>(place);

            return viewmodelList[0];
         }
    }
}
