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
        public BasicPropertyViewModel(Property property)
        {
            Id = property.Id;
            Title = property.Title;
            Description = property.Description;
            Order = property.Order;
            TravelId = property.Travel.Id;
            Symbol = property.Symbol;
        }

        

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TravelId { get; set; }
        public int Order { get; set; }
        public string Symbol { get; set; }
    }
}
