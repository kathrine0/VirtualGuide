using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.Converters
{
    public class IsOwnedToOpacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isOwned = (bool)value;

            if (isOwned)
            {
                return 1;
            }
            else
            {
                return 0.5;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((double)value == 1)
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
