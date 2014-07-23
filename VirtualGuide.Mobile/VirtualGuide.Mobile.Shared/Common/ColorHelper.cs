using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.Common
{
    class ColorHelper
    {
        public static Brush[] TileColors = new Brush[]
        {
            new SolidColorBrush(Colors.LightBlue),
            new SolidColorBrush(Colors.LightGreen),
            new SolidColorBrush(Colors.LightYellow),
            //new SolidColorBrush(Application.Current.Resources["LightViolet"]),
        };
    }

    
}
