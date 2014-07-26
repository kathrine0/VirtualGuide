using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Model;
using VirtualGuide.Mobile.ViewModel;

namespace VirtualGuide.Mobile.Repository
{
    public class PropertyRepository
    {
        private async Task<List<Property>> GetPropertiesByTravelIdAsync(int TravelId)
        {
            var properties = await App.Connection.QueryAsync<Property>("Select * FROM Property WHERE TravelId = ? ORDER BY ItemsOrder ASC", TravelId);

            return properties;
        }

        public async Task<List<PropertyViewModel>> GetSimpleProperties(int travelId)
        {
            var properties = await GetPropertiesByTravelIdAsync(travelId);

            return ModelHelper.ObjectToViewModel<PropertyViewModel, Property>(properties);
        }
    }
}
