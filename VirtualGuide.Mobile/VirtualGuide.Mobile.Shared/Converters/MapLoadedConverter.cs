using System;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.Converters
{
    public class MapEventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var map = parameter as MapControl; 
            if (map != null)
            {
                return parameter;
            }

            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }

}
