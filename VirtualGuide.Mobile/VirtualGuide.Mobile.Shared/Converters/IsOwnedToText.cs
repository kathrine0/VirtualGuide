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
                return App.ResLoader.GetString("Downloaded");
            }
            else
            {
                return App.ResLoader.GetString("Recommended");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((string)value == App.ResLoader.GetString("Downloaded"))
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
