using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VirtualGuide.Models
{
    public class User : ApplicationUser
    {
        public User() : base()
        {
            PurchasedTravels = new List<User_Purchased_Travel>();
            Travels = new List<Travel>();
        }

        //public User(string firstname)
        //    : base(firstname)
        //{
        //    PurchasedTravels = new List<User_Purchased_Travel>();
        //    Travels = new List<Travel>();
        //}

        //[EmailAddressAttribute]
        //public override string UserName
        //{
        //    get
        //    {
        //        return base.UserName;
        //    }
        //    set
        //    {
        //        base.UserName = value;
        //    }
        //}
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public virtual ICollection<User_Purchased_Travel> PurchasedTravels { get; set; }

        [InverseProperty("Creator")]
        public virtual ICollection<Travel> Travels { get; set; }
    }
}