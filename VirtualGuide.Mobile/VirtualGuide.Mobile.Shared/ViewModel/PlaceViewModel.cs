using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using VirtualGuide.Mobile.Helper;
using System.Collections.ObjectModel;
using VirtualGuide.Mobile.Model;

namespace VirtualGuide.Mobile.ViewModel
{


    [ImplementPropertyChanged]
    public class ListPlaceViewModel
    {
        public ListPlaceViewModel() { }
        public ListPlaceViewModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Category = place.Category;
            _placeLatitude = place.Latitude;
            _placeLongitude = place.Longitude;
        }

        private double _placeLatitude;
        private double _placeLongitude;

        public int Id { get; set; }

        public string Name { get; set; }

        public double? Distance { get; set; }

        public string Category { get; set; }

        public List<ListPlaceViewModel> Data
        {
            get;
            set;
        }

        private CollectionViewSource _collection;
        public CollectionViewSource Collection
        {
            get
            {
                _collection = new CollectionViewSource();
                if (Data != null)
                {
                    var grouped = Data.ToGroups(x => x.Name, x => x.Category, true);
                    _collection.Source = grouped;
                    _collection.IsSourceGrouped = true;
                }
                return _collection;
            }
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
    }

    [ImplementPropertyChanged]
    public class PlaceViewModel
    {
        public PlaceViewModel() { }
        public PlaceViewModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Category = place.Category;
            Description = place.Description;
            _placeLatitude = place.Latitude;
            _placeLongitude = place.Longitude;
            _imageSrc = place.ImageSrc;
        }

        private double _placeLatitude;
        private double _placeLongitude;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

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

        public string Category { get; set; }

    }
}
