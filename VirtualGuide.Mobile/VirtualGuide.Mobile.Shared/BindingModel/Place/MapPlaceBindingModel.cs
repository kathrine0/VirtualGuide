using GalaSoft.MvvmLight.Command;
using PropertyChanged;
using System;
using VirtualGuide.Mobile.Model;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.BindingModel
{
    [ImplementPropertyChanged]
    public class MapPlaceBindingModel
    {
        public RelayCommand<MapPlaceBindingModel> ShowDetailCloudCommand { get; set; }
        public RelayCommand<MapPlaceBindingModel> NavigateToPlaceDetailsCommand { get; set; }

        public MapPlaceBindingModel() { }

        public MapPlaceBindingModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            CategoryName = place.Category;
            Point = new Geopoint(new BasicGeoposition() { Latitude = place.Latitude, Longitude = place.Longitude });
            _iconName = place.IconName;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public CategoryVisibilityModel Category
        {
            get;
            set;
        }

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
