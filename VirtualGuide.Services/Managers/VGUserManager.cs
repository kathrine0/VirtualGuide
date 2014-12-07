using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using VirtualGuide.Models;

namespace VirtualGuide.Services.Managers
{
    public class VGUserManager : BaseVGUserManager
    {
        public VGUserManager() : base()
        {

        }

        public static VGUserManager Create(IdentityFactoryOptions<VGUserManager> options, IOwinContext context)
        {
            var manager = new VGUserManager();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
