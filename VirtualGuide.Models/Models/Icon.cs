using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Models
{
    public class Icon
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
