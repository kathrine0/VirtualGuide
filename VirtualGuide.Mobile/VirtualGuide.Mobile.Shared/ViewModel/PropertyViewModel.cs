using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.ViewModel
{
    class PropertyViewModel
    {
        private async Task<List<Property>> GetItemsFromDb(int TravelId)
        {
            var properties = await App.Connection.QueryAsync<Property>("Select * FROM Property WHERE TravelId = ? ORDER BY ItemsOrder ASC", TravelId);

            return properties;
        }

        public async Task<List<SimplePropertyViewModel>> GetSimpleProperties(int travelId)
        {
            var properties = await GetItemsFromDb(travelId);
            var result = new List<SimplePropertyViewModel>();

            foreach (var property in properties)
            {
                result.Add(new SimplePropertyViewModel(property));
            }

            return result;
        }
    }

    class SimplePropertyViewModel
    {
        public SimplePropertyViewModel()
        {

        }

        public SimplePropertyViewModel(Property property)
        {
            Name = property.Title;

        }

        public string Name { get; set; }
        public Brush Background { get; set; }
        public string SymbolCode { get; set; }
    }

}
