using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtualGuide.Mobile.Model;
using System.Net.Http;
using System.Threading.Tasks;
using VirtualGuide.Mobile.Common;
using PropertyChanged;

namespace VirtualGuide.Mobile.ViewModel
{

    [ImplementPropertyChanged]
    public class TravelViewModel
    {
        public TravelViewModel()
        {

        }

        public TravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            Price = travel.Price;
            Latitude = travel.Latitude;
            Longitude = travel.Longitude;
            ZoomLevel = travel.ZoomLevel;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public List<Property> Properties { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double ZoomLevel { get; set; }
    }
}
