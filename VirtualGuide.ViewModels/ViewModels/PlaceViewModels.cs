using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.ViewModels
{
    //TODO Automapper!
    public class MobilePlaceViewModel
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

        public string Category { get; set; }

        public string IconName { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        public virtual IList<MobilePlaceViewModel> Children { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public int TravelId { get; set; }
    }

    public class BasicPlaceViewModel
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
