﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VirtualGuide.Models
{
    public class User : IdentityUser
    {
        public User() : base()
        {
            PurchasedTravels = new List<User_Purchased_Travel>();
            Travels = new List<Travel>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(BaseVGUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(BaseVGUserManager manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<User_Purchased_Travel> PurchasedTravels { get; set; }

        [InverseProperty("Creator")]
        public virtual ICollection<Travel> Travels { get; set; }
    }
}