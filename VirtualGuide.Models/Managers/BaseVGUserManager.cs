using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Models
{
    public class BaseVGUserManager : UserManager<User>
    {
        public BaseVGUserManager(): base(new UserStore<User>(new ApplicationDbContext()))
        {
            UserValidator = new UserValidator<User>(this)
            { 
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            //UserLockoutEnabledByDefault = true;
            //DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //MaxFailedAccessAttemptsBeforeLockout = 5;
        }
    }
}
