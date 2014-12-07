using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Required]
        public int TravelId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}