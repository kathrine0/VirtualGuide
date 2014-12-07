using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtualGuide.BindingModels
{
    /// <summary>
    /// Basic model for the list of Travels
    /// </summary>
    public class BasicTravelBindingModel
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
    public class CustomerTravelBindingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual IList<MobilePlaceBindingModel> Places { get; set; }

        public virtual IList<BasicExcursionBindingModels> Excursions { get; set; }

        public virtual IList<MobilePropertyBindingModel> Properties { get; set; }

        public string Language { get; set; }

        public string ImageSrc { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double ZoomLevel { get; set; }
    }

    /// <summary>
    /// Model for travels creator
    /// </summary>
    public class CreatorTravelBindingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(30, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(100, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Description { get; set; }

        public virtual IList<BasicPlaceBindingModel> Places { get; set; }

        public virtual IList<BasicExcursionBindingModels> Excursions { get; set; }

        public virtual IList<BasicPropertyBindingModel> Properties { get; set; }

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
    public class SimpleCreatorTravelBindingModel
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