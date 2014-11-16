using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Models
{
    public class VGUserManager : UserManager<User>
    {
        public VGUserManager(): base(new UserStore<User>(new ApplicationDbContext()))
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

        //public static VGUserManager Create(IdentityFactoryOptions<VGUserManager> options, IOwinContext context)
        //{
        //    var manager = new VGUserManager();

        //    var dataProtectionProvider = options.DataProtectionProvider;
        //    if (dataProtectionProvider != null)
        //    {
        //        manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        //    }
        //    return manager;
        //}
    }
}
