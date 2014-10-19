using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VirtualGuide.Models
{
    public class Place :IAuditable
    {
        public Place()
        {
            Properties = new List<Property>();
            Children = new List<Place>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public IList<Property> Properties { get; set; }

        public virtual IList<Place> Children { get; set; }

        public virtual Place Parent { get; set; }
        public int? ParentId { get; set; }

        //[Required]
        [ForeignKey("TravelId")]
        public virtual Travel Travel { get; set; }
        
        public int TravelId { get; set; }

        //[Required]
        [ForeignKey("CategoryId")]
        public virtual PlaceCategory Category { get; set; }

        public int CategoryId { get; set; }

        public string ImageSrc { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}