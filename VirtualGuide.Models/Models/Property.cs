using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VirtualGuide.Models
{
    public class Property : IAuditable
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(35)]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        public int Order { get; set; }
        [Required]
        public int TravelId { get; set; }
        
        [ForeignKey("TravelId")]
        public virtual Travel Travel { get; set; }
        
        [Required]
        public int IconId { get; set; }
        
        [ForeignKey("IconId")]
        public virtual Icon Icon { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}