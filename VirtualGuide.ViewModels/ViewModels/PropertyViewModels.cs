using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.ViewModels
{
    public class BasicPropertyViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(35)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int TravelId { get; set; }

        public int Order { get; set; }

        public IconViewModel Icon { get; set; }

        [Required]
        public int IconId { get; set; }
    }
}
