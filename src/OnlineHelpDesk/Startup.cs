using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineHelpDesk.Startup))]
namespace OnlineHelpDesk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
