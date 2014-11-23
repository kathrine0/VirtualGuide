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
    public class PropertyRepository
    {
        private async Task<List<Property>> GetPropertiesByTravelIdAsync(int TravelId)
        {
            var query = App.Connection.QueryAsync<Property>("Select * FROM Property WHERE TravelId = ? ORDER BY ItemsOrder ASC", TravelId);

            var properties = await query.ConfigureAwait(false);
            return properties;
        }

        public async Task<List<T>> GetSimpleProperties<T>(int travelId)
        {
            var properties = await GetPropertiesByTravelIdAsync(travelId);

            return ModelHelper.ObjectToViewModel<T, Property>(properties);
        }
    }
}
