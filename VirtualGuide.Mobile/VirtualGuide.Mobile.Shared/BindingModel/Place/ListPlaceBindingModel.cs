using PropertyChanged;
using System;
using System.Collections.Generic;
using VirtualGuide.Mobile.Helper;
using VirtualGuide.Mobile.Model;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace VirtualGuide.Mobile.BindingModel
{

    [ImplementPropertyChanged]
    public class ListPlaceBindingModel
    {
        public ListPlaceBindingModel(double latitude, double longitude)
        {
            _placeLatitude = latitude;
            _placeLongitude = longitude;
        }

        private double _placeLatitude;
        private double _placeLongitude;

        public int Id { get; set; }

        public string Name { get; set; }

        public double? Distance { get; set; }

        public string Category { get; set; }

        public List<ListPlaceBindingModel> Data
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
        /// This is Ortodroma formula
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public void SetDistance(Geoposition currentPosition)
        {
            const double degreesToRadians = (Math.PI / 180.0);
            const double earthRadius = 6371; // kilometers

            // convert latitude and longitude values to radians
            double prevRadLat = _placeLatitude * degreesToRadians;
            double prevRadLong = _placeLongitude * degreesToRadians;
            double currRadLat = currentPosition.Coordinate.Point.Position.Latitude * degreesToRadians;
            double currRadLong = currentPosition.Coordinate.Point.Position.Longitude * degreesToRadians;

            // calculate ortodroma
            var exp1 = Math.Pow((Math.Sin((prevRadLat - currRadLat) / 2)), 2);
            var exp2 = Math.Cos(prevRadLat)*Math.Cos(currRadLat)*Math.Pow(Math.Sin((prevRadLong-currRadLong)/2) ,2);
            var ortodroma = 2 * earthRadius * Math.Asin(Math.Sqrt(exp1+exp2));

            // calculate radian delta between each position.            
            this.Distance = ortodroma * 1000;  // return results as meters
        }
    }
}
