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
        public async Task<List<T>> GetParentPlacesByTravelIdAsync<T>(int travelId)
        {
            var places = await App.Connection.QueryAsync<Place>("Select * FROM Place WHERE TravelId = ? AND (ParentId IS NULL OR ParentId = '' ) ORDER BY Latitude DESC", travelId);

            return ModelHelper.ObjectToViewModel<T, Place>(places);
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
