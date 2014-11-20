﻿using System;
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

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        [StringLength(35, ErrorMessageResourceName="StringTooLong", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = "ValueNotNull", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public int TravelId { get; set; }

        public int Order { get; set; }

        public IconViewModel Icon { get; set; }

        [Required(ErrorMessageResourceName = "ChooseIcon", ErrorMessageResourceType = typeof(Common.Translation.Resources))]
        public int IconId { get; set; }
    }

    public class MobilePropertyViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int TravelId { get; set; }

        public int Order { get; set; }

        public string IconSymbol { get; set; }

        public int IconId { get; set; }
    }
}
