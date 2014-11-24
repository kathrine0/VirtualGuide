using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.BindingModel;
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

        public async Task<List<GuideMainPropertyBindingModel>> GetSimplePropertiesWithColors(int travelId)
        {
            var properties = await GetPropertiesByTravelIdAsync(travelId);

            var viewModels = new List<GuideMainPropertyBindingModel>();

            for (int i = 0; i < properties.Count; i++ )
            {
                viewModels.Add(new GuideMainPropertyBindingModel(properties[i], i));
            }

            return viewModels;
        }
    }
}
