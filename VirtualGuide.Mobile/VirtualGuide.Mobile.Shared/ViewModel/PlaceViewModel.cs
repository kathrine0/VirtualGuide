using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Model;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VirtualGuide.Mobile.ViewModel
{
    [ImplementPropertyChanged]
    public class MapPlaceViewModel
    {
        public MapPlaceViewModel() {}

        public MapPlaceViewModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Point = new Geopoint(new BasicGeoposition() {Latitude = place.Latitude, Longitude = place.Longitude});
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public Geopoint Point { get; set; }

        public bool DetailsVisibility { get; set; }
    }

    [ImplementPropertyChanged]
    public class PlaceViewModel
    {
        public PlaceViewModel() { }
        public PlaceViewModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Description = place.Description;
            _placeLatitude = place.Latitude;
            _placeLongitude = place.Longitude;
            _imageSrc = place.ImageSrc;
        }

        /// <summary>
        /// This is Haversine formula
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public void SetDistance(Geoposition currentPosition)
        {
            const double degreesToRadians = (Math.PI / 180.0);
            const double earthRadius = 6371; // kilometers

            // convert latitude and longitude values to radians
            var prevRadLat = _placeLatitude * degreesToRadians;
            var prevRadLong = _placeLongitude * degreesToRadians;
            var currRadLat = currentPosition.Coordinate.Latitude * degreesToRadians;
            var currRadLong = currentPosition.Coordinate.Longitude * degreesToRadians;

            // calculate radian delta between each position.
            var radDeltaLat = currRadLat - prevRadLat;
            var radDeltaLong = currRadLong - prevRadLong;

            // calculate distance
            var expr1 = (Math.Sin(radDeltaLat / 2.0) *
                         Math.Sin(radDeltaLat / 2.0)) +

                        (Math.Cos(prevRadLat) *
                         Math.Cos(currRadLat) *
                         Math.Sin(radDeltaLong / 2.0) *
                         Math.Sin(radDeltaLong / 2.0));

            var expr2 = 2.0 * Math.Atan2(Math.Sqrt(expr1),
                                         Math.Sqrt(1 - expr1));

            var distance = (earthRadius * expr2);
            this.Distance = distance * 1000;  // return results as meters
        }

        private double _placeLatitude;
        private double _placeLongitude;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double? Distance { get; set; }

        private string _imageSrc;

        private ImageSource _imagePath;
        public ImageSource ImagePath
        {
            get
            {
                ImageSource bitmap = null;
                Uri uri = null;
                try
                {
                    uri = new Uri("ms-appdata:///local/images/" + _imageSrc);
                    bitmap = new BitmapImage(uri);
                    _imagePath = bitmap;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
                return _imagePath;
            }
        }
    }
}
