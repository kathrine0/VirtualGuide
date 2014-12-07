using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;

namespace VirtualGuide.Mobile.Repository
{
    class PlaceRepository
    {
        public async Task<List<MapPlaceBindingModel>> GetParentPlacesByTravelIdAsync(int travelId)
        {
            var query = App.Connection.QueryAsync<Place>("Select * FROM Place WHERE TravelId = ? AND (ParentId IS NULL OR ParentId = '' ) ORDER BY Latitude DESC", travelId);
            var places = await query.ConfigureAwait(false);

            return AutoMapper.Mapper.Map<List<MapPlaceBindingModel>>(places);
        }

        public async Task<PlaceMainBindingModel> GetPlaceById(int placeId)
        {
            var query = App.Connection.QueryAsync<Place>("Select * FROM Place WHERE Id = ?", placeId);
            var place = await query.ConfigureAwait(false);

            if (place.Count == 0)
                throw new Exception("Element not found");

            return AutoMapper.Mapper.Map<PlaceMainBindingModel>(place.First());
        }

        public ItemsChangeObservableCollection<CategoryVisibilityModel> GetCategoryVisibilityCollection(List<MapPlaceBindingModel> places)
        {
            IEnumerable<string> categories = places.Select(x => x.CategoryName).Distinct();
            ItemsChangeObservableCollection<CategoryVisibilityModel> result = new ItemsChangeObservableCollection<CategoryVisibilityModel>();

            foreach (var category in categories)
            {
                result.Add(new CategoryVisibilityModel() { Visibile = true, Name = category });
            }

            return result;
        }
    }
}
