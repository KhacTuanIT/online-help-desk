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

            // Tao Role va User Mac dinh
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Create Super Admin role and default sa account
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                // Create SuperAdmin Role
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SuperAdmin";
                roleManager.Create(role);

                // Init default super admin account
                var u = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@ohd.com",
                    FullName = "Super Admin"
                };

                string saPassword = "ADM@123a";

                var checkUser = UserManager.Create(u, saPassword);

                //Add default User to Role Admin
                if (checkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(u.Id, "SuperAdmin");
                }
            }

            // Create role Student
            if (!roleManager.RoleExists("Student"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);
            }

            // Create role Assignor
            if (!roleManager.RoleExists("Assignor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Assignor";
                roleManager.Create(role);
            }

            // Create role FacilityHead
            if (!roleManager.RoleExists("FacilityHead"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "FacilityHead";
                roleManager.Create(role);
            }
        }
    }
}
