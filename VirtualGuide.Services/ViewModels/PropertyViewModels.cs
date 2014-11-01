using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services
{
    public class BasicPropertyViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TravelId { get; set; }
        public int Order { get; set; }
        public Icon Icon { get; set; }

        public int? IconId { get; set; }
    }
}
