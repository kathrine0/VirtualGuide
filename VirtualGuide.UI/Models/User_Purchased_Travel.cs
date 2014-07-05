using System;
using System.Collections.Generic;
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
        public virtual Travel Travel { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}