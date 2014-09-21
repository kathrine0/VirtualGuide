using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VirtualGuide.WebApi.Startup))]
namespace VirtualGuide.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
