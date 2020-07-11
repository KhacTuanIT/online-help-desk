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
            DatabaseHelper.InitializeIdentity();

            // Initialize default facilities
            DatabaseHelper.InitializeFacility();

            // Initialize default status type
            DatabaseHelper.InitializeStatusType();

            // Initialize default request type
            DatabaseHelper.InitializeRequestType();

            // Initialize default equipment type
            DatabaseHelper.InitializeEquipmentType();

            // Initialize default equipments
            DatabaseHelper.InitializeEquipment();
        }
    }
}
