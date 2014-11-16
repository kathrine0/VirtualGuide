using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Models
{
    public class PlaceCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [RegularExpression("^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }

        [Required]
        public string IconName { get; set; }
    }
}
