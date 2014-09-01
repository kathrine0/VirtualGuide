using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services
{
    public class BasicPlaceViewModel
    {
        public BasicPlaceViewModel()
        {
            Properties = new List<BasicPropertyViewModel>();
            Children = new List<BasicPlaceViewModel>();
        }

        public BasicPlaceViewModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Description = place.Description;
            Latitude = place.Latitude;
            Longitude = place.Longitude;
            ImageSrc = place.ImageSrc;
            Category = place.Category == null ? string.Empty : place.Category.Name;
         
            Properties = ServicesHelper.CreateViewModelListFromModel<BasicPropertyViewModel, Property>(place.Properties);
            Children = ServicesHelper.CreateViewModelListFromModel<BasicPlaceViewModel, Place>(place.Children);

            ParentId = place.ParentId;
            TravelId = place.Travel.Id;

        }

        public int Id { get; set; }

      
        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageSrc { get; set; }

        public string Category { get; set; }

        public IList<BasicPropertyViewModel> Properties { get; set; }

        public virtual IList<BasicPlaceViewModel> Children { get; set; }

        public int? ParentId { get; set; }
        public int TravelId { get; set; }
    }
}
