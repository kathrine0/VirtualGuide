using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VirtualGuide.Models
{
    public class User_Purchased_Travel :IAuditable
    {
        public User_Purchased_Travel()
        {
        }
        public int Id { get; set; }
       
        [ForeignKey("TravelId")]
        public virtual Travel Travel { get; set; }
        public int TravelId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}