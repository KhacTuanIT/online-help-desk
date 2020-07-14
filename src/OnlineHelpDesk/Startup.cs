using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

using OnlineHelpDesk.Models;

[assembly: OwinStartupAttribute(typeof(OnlineHelpDesk.Startup))]
namespace OnlineHelpDesk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Initialize default roles and admin
            DatabaseHelper.InitializeRequiredData();
        }
    }
}
