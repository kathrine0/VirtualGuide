//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VirtualGuide.UI
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Purchased_Travel
    {
        public int Id { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public Nullable<int> Travel_Id { get; set; }
        public string User_Id { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Travels Travels { get; set; }
    }
}
