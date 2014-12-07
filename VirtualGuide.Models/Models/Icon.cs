using System.ComponentModel.DataAnnotations;

namespace VirtualGuide.Models
{
    public class Icon
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
