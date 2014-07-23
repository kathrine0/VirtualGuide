using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtualGuide.Models;

namespace VirtualGuide.Services
{
    public class BasicTravelViewModel
    {
        public BasicTravelViewModel()
        {

        }
        public BasicTravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            Price = travel.Price;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        //[RegularExpression(@"^[a-z]{2}-[A-Z]{2}$")]
        public string Language { get; set; }

        public double Price { get; set; }
    }

    public class ComplexReadTravelViewModel
    {
        public ComplexReadTravelViewModel(Travel travel)
        {
            Id = travel.Id;
            Name = travel.Name;
            Description = travel.Description;
            //TODO Places = 
            //TODO Excursions =
            Properties = BasicPropertyViewModel.CreatePropertyList(travel.Properties);
            Language = travel.Language;

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual IList<BasicPlaceViewModel> Places { get; set; }

        public virtual IList<BasicExcursionViewModel> Excursions { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        public string Language { get; set; }
    }
}