﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VirtualGuide.ViewModels
{
    /// <summary>
    /// Basic model for the list of Travels
    /// </summary>
    public class BasicTravelViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [RegularExpression("^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ImageSrc { get; set; }

    }

    /// <summary>
    /// Model that the end user gets with all the details
    /// </summary>
    public class CustomerTravelViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public virtual IList<MobilePlaceViewModel> Places { get; set; }

        public virtual IList<BasicExcursionViewModel> Excursions { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        [Required]
        [RegularExpression("^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }

        [Required]
        public string ImageSrc { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double ZoomLevel { get; set; }
    }

    /// <summary>
    /// Model for travels creator
    /// </summary>
    public class CreatorTravelViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public virtual IList<BasicPlaceViewModel> Places { get; set; }

        public virtual IList<BasicExcursionViewModel> Excursions { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [RegularExpression("^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }

        [Required]
        public string ImageSrc { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double ZoomLevel { get; set; }
     
    }

    /// <summary>
    /// Model for travels creator without any collections (used for edit only)
    /// </summary>
    public class SimpleCreatorTravelViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [RegularExpression("^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }

        [Required]
        public string ImageSrc { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double ZoomLevel { get; set; }

    }
}