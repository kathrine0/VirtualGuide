//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VirtualGuide.WebApi
{
    using System;
    using System.Collections.Generic;
    
    public partial class Place_Excursion
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public Nullable<int> Place_Id { get; set; }
        public Nullable<int> Excursion_Id { get; set; }
    
        public virtual Excursions Excursions { get; set; }
        public virtual Places Places { get; set; }
    }
}
