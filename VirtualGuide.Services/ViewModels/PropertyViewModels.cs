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
        public BasicPropertyViewModel()
        {

        }
        public BasicPropertyViewModel(Property property)
        {
            Id = property.Id;
            Title = property.Title;
            Description = property.Description;
            Order = property.Order;
            TravelId = property.Travel.Id;
            Icon = property.Icon;
        }


        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TravelId { get; set; }
        public int Order { get; set; }
        public Icon Icon { get; set; }

        public Property ToModel()
        {
            return new Property()
            {
                Id = this.Id,
                Title = this.Title,
                Description = this.Description,
                TravelId = this.TravelId,
                Order = this.Order,
                IconId = this.Icon.Id
            };
        }
    }
}
