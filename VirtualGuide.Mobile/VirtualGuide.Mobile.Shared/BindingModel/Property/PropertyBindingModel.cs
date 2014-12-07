using PropertyChanged;
using System.Collections.Generic;
using System.Net;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class PropertyBindingModel
    {
        public enum Types
        {
            MAPS, TOURS, REGULAR
        }
        
        #region constructors

        public PropertyBindingModel()
        {
            Type = Types.REGULAR;

            Background = ColorHelper.YELLOW;           
        }

        #endregion

        #region properties

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
        public Brush Background 
        { 
            get; 
            set; 
        }

        private string _iconSymbol = string.Empty;
        public string Icon { 
            get
            {
                return WebUtility.HtmlDecode(_iconSymbol);
            }
            set
            {
                _iconSymbol = value;
            }
        }
        public Types Type { get; set; }

        #endregion
        
    }
}
