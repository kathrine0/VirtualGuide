using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VirtualGuide.Models
{
    public class BasicTravelViewModel
    {
        public BasicTravelViewModel()
        {

        }
        public BasicTravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            Price = travel.Price;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public double Price { get; set; }
    }


    public class PropertiesTravelViewModel
    {
        public PropertiesTravelViewModel()
        {

        }

        public PropertiesTravelViewModel(int travelId)
        {
            Id = travelId;
            Properties = new List<Property>();
        }

        public PropertiesTravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Properties = travel.Properties;
        }

        public int Id { get; set; }
        public IList<Property> Properties { 
            get; 
            set; 
        }
    }

    public class PropertyTravelViewModel
    {
        public PropertyTravelViewModel()
        {
              
        }
        public PropertyTravelViewModel(int TravelId)
        {
            this.TravelId = TravelId;
        }

        public PropertyTravelViewModel(Property property)
        {
            this.Id = property.Id;
            this.Title = property.Title;
            this.Description = property.Description;
            this.TravelId = property.Travel.Id;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int TravelId { get; set; }
    }

    public class PlacesTravelViewModel
    {
        public PlacesTravelViewModel()
        {

        }
        public PlacesTravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Places = travel.Places;
        }

        public int Id { get; set; }
        public IList<Place> Places { get; set; }
    }

    public class PlaceTravelViewModel
    {
        public PlaceTravelViewModel()
        {

        }

        public PlaceTravelViewModel(int TravelId)
        {
            this.TravelId = TravelId;
        }

        public PlaceTravelViewModel(Place place)
        {
            this.Id = place.Id;
            this.Name = place.Name;
            this.Description = place.Description;
            this.Gps = place.Gps;
            this.Radius = place.Radius;
            this.TravelId = place.Travel.Id;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public double Gps { get; set; }

        public double Radius { get; set; }

        public int TravelId { get; set; }
    }
}