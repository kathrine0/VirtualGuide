using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.ViewModels
{
    public class IconViewModel
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
