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
        public ListPlaceBindingModel() { }
        public ListPlaceBindingModel(Place place)
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
}
