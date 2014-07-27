﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtualGuide.Models;

namespace VirtualGuide.Services
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
            ImageSrc = travel.ImageSrc;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        //[RegularExpression(@"^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }

        public double Price { get; set; }
        public string ImageSrc { get; set; }

    }

    public class ComplexReadTravelViewModel
    {
        public ComplexReadTravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            Places = ServicesHelper.CreateViewModelListFromModel<BasicPlaceViewModel, Place>(travel.Places);
            //TODO Excursions =
            Properties = ServicesHelper.CreateViewModelListFromModel<BasicPropertyViewModel, Property>(travel.Properties);
            Language = travel.Language;
            Latitude = travel.Latitude;
            Longitude = travel.Longitude;
            ZoomLevel = travel.ZoomLevel;
            ImageSrc = travel.ImageSrc;

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual IList<BasicPlaceViewModel> Places { get; set; }

        public virtual IList<BasicExcursionViewModel> Excursions { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        public string Language { get; set; }
        public string ImageSrc { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double ZoomLevel { get; set; }
    }
}