﻿using System;
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
        [StringLength(100)]
        public string Title { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int Order { get; set; }
        public int? TravelId { get; set; }
        
        [ForeignKey("TravelId")]
        public virtual Travel Travel { get; set; }
        public int? SymbolId { get; set; }
        [ForeignKey("SymbolId")]

        public Icon Symbol { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}