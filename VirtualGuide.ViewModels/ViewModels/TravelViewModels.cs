using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualGuide.ViewModels
{
    /// <summary>
    /// Basic model for the list of Travels
    /// </summary>
    public class BasicTravelViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(30, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(100, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [RegularExpression("^[a-z]{2}_[A-Z]{2}$",
            ErrorMessageResourceName = "InvalidLanguageFormat",
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Language { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Price { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string ImageSrc { get; set; }

        public bool ApprovalStatus { get; set; }

    }

    /// <summary>
    /// Model that the end user gets with all the details
    /// </summary>
    public class CustomerTravelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual IList<MobilePlaceViewModel> Places { get; set; }

        public virtual IList<BasicExcursionViewModel> Excursions { get; set; }

        public virtual IList<MobilePropertyViewModel> Properties { get; set; }

        public string Language { get; set; }

        public string ImageSrc { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double ZoomLevel { get; set; }
    }

    /// <summary>
    /// Model for travels creator
    /// </summary>
    public class CreatorTravelViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(30, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(100, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Description { get; set; }

        public virtual IList<BasicPlaceViewModel> Places { get; set; }

        public virtual IList<BasicExcursionViewModel> Excursions { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Price { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [RegularExpression("^[a-z]{2}_[A-Z]{2}$",
            ErrorMessageResourceName = "InvalidLanguageFormat",
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Language { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string ImageSrc { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Latitude { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Longitude { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double ZoomLevel { get; set; }
     
    }

    /// <summary>
    /// Model for travels creator without any collections (used for edit only)
    /// </summary>
    public class SimpleCreatorTravelViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", 
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(30, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull",
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(100, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", 
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Price { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", 
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [RegularExpression("^[a-z]{2}_[A-Z]{2}$", 
            ErrorMessageResourceName="InvalidLanguageFormat",
            ErrorMessageResourceType=typeof(Common.Translation.Resources))]
        public string Language { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", 
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string ImageSrc { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", 
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Latitude { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", 
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Longitude { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", 
            ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double ZoomLevel { get; set; }

    }
}