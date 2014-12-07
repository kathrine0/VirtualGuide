using PropertyChanged;
using System.Collections.Generic;
using System.Net;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class GuideMainPropertyBindingModel
    {
        public enum Types
        {
            MAPS, TOURS, REGULAR
        }
        
        #region constructors
        public GuideMainPropertyBindingModel() { }

        public GuideMainPropertyBindingModel(Property property)
        {
            Id = property.Id;
            Name = property.Title;
            Icon = property.IconSymbol;
            Type = Types.REGULAR;
            Description = property.Description;

            if (Background == null)
            {
                Background = ColorHelper.YELLOW;
            }

        }

        public GuideMainPropertyBindingModel(Property property, int i) : this(property)
        {
            List<Brush> colors = new List<Brush>() { ColorHelper.BLUE, ColorHelper.GREEN, ColorHelper.RED };

            Background = colors[i % colors.Count];
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
