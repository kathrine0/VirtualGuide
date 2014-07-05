using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualGuide.Models
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}