using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.ViewModel
{

    [ImplementPropertyChanged]
    public class PropertyViewModel
    {
        public enum Types
        {
            MAPS, TOURS, REGULAR
        }

        public PropertyViewModel() { }

        public PropertyViewModel(Property property)
        {
            Id = property.Id;
            Name = property.Title;
            Symbol = property.Symbol;
            Background = ColorHelper.YELLOW;
            Type = Types.REGULAR;
            Description = property.Description;

        }
        public int Id { get; set; }

        private string _name = string.Empty;
        public string Name {
            get
            {
                return _name.ToUpper();
            } 
            set
            {
                _name = value;
            }
        }
        public string Description { get; set; }
        public Brush Background { get; set; }

        private string _symbol = string.Empty;
        public string Symbol { 
            get
            {
                return WebUtility.HtmlDecode(_symbol);
            }
            set
            {
                _symbol = value;
            }
        }
        public Types Type { get; set; }
    }

}
