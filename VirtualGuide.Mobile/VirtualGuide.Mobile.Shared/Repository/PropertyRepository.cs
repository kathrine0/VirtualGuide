using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
using VirtualGuide.Mobile.Model;

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

        public async Task<List<PropertyBindingModel>> GetSimpleProperties(int travelId)
        {
            var properties = await GetPropertiesByTravelIdAsync(travelId);
            
            return AutoMapper.Mapper.Map<List<PropertyBindingModel>>(properties);
        }
    }
}
