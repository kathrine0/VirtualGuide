using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.Converters
{
    public class FalseIsVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!((bool)value))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Collapsed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
