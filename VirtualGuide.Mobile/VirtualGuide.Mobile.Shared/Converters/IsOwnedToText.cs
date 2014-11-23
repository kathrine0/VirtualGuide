using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.Converters
{
    public class IsOwnedToText : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isOwned = (bool)value;

            if (isOwned)
            {
                return "Your guides";
            }
            else
            {
                return "See also";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((string)value == "Your guides")
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
