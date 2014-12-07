using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(VirtualGuide.WebService.Startup))]

namespace VirtualGuide.WebService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
