using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VirtualGuide.UI.Startup))]
namespace VirtualGuide.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
