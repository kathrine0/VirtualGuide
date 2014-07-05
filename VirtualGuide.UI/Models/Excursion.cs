using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VirtualGuide.Models
{
    public class Excursion : IAuditable
    {
        public Excursion()
        {
            Properties = new List<Property>();
            Places = new List<Place_Excursion>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IList<Property> Properties { get; set; }

        public IList<Place_Excursion> Places { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual Travel Travel { get; set; }
    }
}