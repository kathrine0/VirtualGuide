using System.ComponentModel.DataAnnotations;

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
        [RegularExpression("^[a-z]{2}_[A-Z]{2}$")]
        public string Language { get; set; }

        [Required]
        public string IconName { get; set; }
    }
}
