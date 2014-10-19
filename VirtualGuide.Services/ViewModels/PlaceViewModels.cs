using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualGuide.Models;

namespace VirtualGuide.Services
{
    public class MobilePlaceViewModel
    {
        public MobilePlaceViewModel()
        {
            Properties = new List<BasicPropertyViewModel>();
            Children = new List<MobilePlaceViewModel>();
        }

        public MobilePlaceViewModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Description = place.Description;
            Latitude = place.Latitude;
            Longitude = place.Longitude;
            ImageSrc = place.ImageSrc;
            Category = place.Category == null ? string.Empty : place.Category.Name;
            IconName = place.Category == null ? string.Empty : place.Category.IconName;

            Properties = ServicesHelper.CreateViewModelListFromModel<BasicPropertyViewModel, Property>(place.Properties);
            Children = ServicesHelper.CreateViewModelListFromModel<MobilePlaceViewModel, Place>(place.Children);

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

        public string IconName { get; set; }

        public IList<BasicPropertyViewModel> Properties { get; set; }

        public virtual IList<MobilePlaceViewModel> Children { get; set; }

        public int? ParentId { get; set; }
        public int TravelId { get; set; }
    }

    public class BasicPlaceViewModel
    {
        public BasicPlaceViewModel()
        {
        }

        public BasicPlaceViewModel(Place place)
        {
            Id = place.Id;
            Name = place.Name;
            Description = place.Description;
            Latitude = place.Latitude;
            Longitude = place.Longitude;
            ImageSrc = place.ImageSrc;
            CategoryId = place.CategoryId;
            CategoryName = place.Category.Name;
            TravelId = place.TravelId;

        }

        public int Id { get; set; }


        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageSrc { get; set; }
        public int CategoryId {get;set;}

        public string CategoryName { get; set; }
        
        public int TravelId { get; set; }

        public Place ToModel()
        {
            return new Place()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                ImageSrc = this.ImageSrc,
                CategoryId = this.CategoryId,
                TravelId = this.TravelId
            };
        }
    }
}
