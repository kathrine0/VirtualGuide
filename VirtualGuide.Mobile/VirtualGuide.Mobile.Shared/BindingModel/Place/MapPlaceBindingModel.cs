using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Model;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class MapPlaceBindingModel
    {
        public DelegateCommand<MapPlaceBindingModel> ShowDetailCloudCommand { get; set; }
        public DelegateCommand<MapPlaceBindingModel> NavigateToPlaceDetailsCommand { get; set; }

        public MapPlaceBindingModel() { }

        public MapPlaceBindingModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Category = place.Category;
            Point = new Geopoint(new BasicGeoposition() { Latitude = place.Latitude, Longitude = place.Longitude });
            _iconName = place.IconName;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        public Geopoint Point { get; set; }

        public bool DetailsVisibility { get; set; }

        private string _iconName;

        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                ImageSource bitmap = null;
                Uri uri = null;
                try
                {
                    uri = new Uri(String.Format("ms-appx:///Assets/Markers/{0}.png", _iconName));
                    bitmap = new BitmapImage(uri);
                    _icon = bitmap;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
                return _icon;
            }
        }

        
    }
}
