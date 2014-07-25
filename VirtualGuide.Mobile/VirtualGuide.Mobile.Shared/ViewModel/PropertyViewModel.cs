using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.ViewModel
{
    [ImplementPropertyChanged]
    public class PropertyViewModel
    {

    }

    [ImplementPropertyChanged]
    public class SimplePropertyViewModel
    {
        public enum Types
        {
            MAPS, TOURS, REGULAR
        }

        public SimplePropertyViewModel() { }

        public SimplePropertyViewModel(Property property)
        {
            Name = property.Title;
            Symbol = property.Symbol;
            Background = ColorHelper.YELLOW;
            Type = Types.REGULAR;

        }
        
        private string _name = String.Empty;
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
        public Brush Background { get; set; }
        public string Symbol { get; set; }
        public Types Type { get; set; }
    }

}
