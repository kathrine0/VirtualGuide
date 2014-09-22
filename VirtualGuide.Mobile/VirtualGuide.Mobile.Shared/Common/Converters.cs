using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.Common
{
    public class TrueIsVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool) value)
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
            if ((Visibility) value == Visibility.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

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

    public class DistanceToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var distance = (double?)value;

            if (distance == null)
            {
                return "? km";
            }
            if (distance > 1000)
            {
                double rnddist = Math.Round(distance.Value/1000, 2);
                return String.Format("{0} km", rnddist.ToString());
            }
            else
            {
                int intdist = (int)distance.Value;
                return String.Format("{0} m", intdist.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            //Don;t really need this
            return 0;
        }
    }

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
            if ((double) value == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

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
