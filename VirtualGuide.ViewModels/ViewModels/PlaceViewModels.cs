using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.ViewModels
{
    //TODO Automapper!
    public class MobilePlaceViewModel
    {
        public MobilePlaceViewModel()
        {
            Properties = new List<BasicPropertyViewModel>();
            Children = new List<MobilePlaceViewModel>();
        }

        public int Id { get; set; }

      
        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageSrc { get; set; }

        public string Category { get; set; }

        public string IconName { get; set; }

        public virtual IList<BasicPropertyViewModel> Properties { get; set; }

        public virtual IList<MobilePlaceViewModel> Children { get; set; }

        public int? ParentId { get; set; }
        public int TravelId { get; set; }
    }

    public class BasicPlaceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageSrc { get; set; }
        public int CategoryId {get;set;}
        public string CategoryName { get; set; }
        public int TravelId { get; set; }
    }
}
