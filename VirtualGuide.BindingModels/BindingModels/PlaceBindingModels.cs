using System.ComponentModel.DataAnnotations;

namespace VirtualGuide.BindingModels
{
    public class MobilePlaceBindingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string ImageSrc { get; set; }

        public string Category { get; set; }

        public string IconName { get; set; }

        public int? ParentId { get; set; }

        public int TravelId { get; set; }
    }

    public class BasicPlaceBindingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(50, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Latitude { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public double Longitude { get; set; }

        public string ImageSrc { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public int CategoryId {get;set;}

        public string CategoryName { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public int TravelId { get; set; }
    }
}
