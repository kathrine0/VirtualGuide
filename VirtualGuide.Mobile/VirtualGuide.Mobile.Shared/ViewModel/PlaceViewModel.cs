﻿using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using VirtualGuide.Mobile.Model;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

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
}
