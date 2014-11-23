using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.Converters
{
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
                double rnddist = Math.Round(distance.Value / 1000, 2);
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
}
