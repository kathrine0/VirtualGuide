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
        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(255, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(10, ErrorMessageResourceName = "StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Symbol { get; set; }
        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Path { get; set; }
    }
}
