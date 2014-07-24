using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<SimplePropertyViewModel>> GetSimpleProperties(int travelId)
        {
            var properties = await GetPropertiesByTravelIdAsync(travelId);
            var result = new List<SimplePropertyViewModel>();

            foreach (var property in properties)
            {
                result.Add(new SimplePropertyViewModel(property));
            }

            return result;
        }
    }
}
