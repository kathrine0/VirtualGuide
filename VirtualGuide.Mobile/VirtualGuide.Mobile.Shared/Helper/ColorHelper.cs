﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace VirtualGuide.Mobile.Helper
{
    class ColorHelper
    {
        public static readonly Brush BLUE = (SolidColorBrush)Application.Current.Resources["ThemeBlue"];
        public static readonly Brush GREEN = (SolidColorBrush)Application.Current.Resources["ThemeGreen"];
        public static readonly Brush YELLOW = (SolidColorBrush)Application.Current.Resources["ThemeYellow"];
        public static readonly Brush RED = (SolidColorBrush)Application.Current.Resources["ThemeRed"];
        public static readonly Brush GRAY = (SolidColorBrush)Application.Current.Resources["ThemeGray"];
        public static readonly Brush LIGHTGRAY = (SolidColorBrush)Application.Current.Resources["ThemeLightGray"];

        public static Brush[] TileColors = new Brush[]
        {
            GRAY, BLUE, GREEN, YELLOW, RED
        };
    }

    
}
