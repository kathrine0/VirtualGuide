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

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string ImageSrc { get; set; }

        public string Category { get; set; }

        public string IconName { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        public virtual IList<MobilePlaceViewModel> Children { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public int TravelId { get; set; }
    }

    public class BasicPlaceViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string ImageSrc { get; set; }

        [Required]
        public int CategoryId {get;set;}

        public string CategoryName { get; set; }

        [Required]
        public int TravelId { get; set; }
    }
}
