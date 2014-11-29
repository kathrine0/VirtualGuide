using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.Converters
{
    public class EmptyIsTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var collection = value as IEnumerable<object>;
            
            if (collection == null || collection.Count() == 0)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
