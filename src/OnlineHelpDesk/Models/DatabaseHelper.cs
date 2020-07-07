using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineHelpDesk.Models
{
    public class DatabaseHelper
    {
        public static void InitializeIdentity()
        {
            using (var context = ApplicationDbContext.Create())
            {
                if (context.Users.Any()) return;

                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                // Add missing roles
                if (!roleManager.RoleExists("SuperAdmin"))
                {
                    // Create new role: Super Admin
                    roleManager.Create(new IdentityRole("SuperAdmin"));

                    // Create default admin
                    var newUser = new ApplicationUser()
                    {
                        UserName = "admin",
                        FullName = "Super Admin",
                        Email = "admin@ohd.com",
                        CreatedAt = DateTime.UtcNow,
                        MustChangePassword = true
                    };
                    userManager.Create(newUser, "admin@123");
                    userManager.SetLockoutEnabled(newUser.Id, false);
                    userManager.AddToRole(newUser.Id, "SuperAdmin");
                    // Note:
                    // - Create new user with role SuperAdmin
                    // - Default password ADM@123a
                    // - He must to change password at first login
                }

                if (!roleManager.RoleExists("Student"))
                {
                    roleManager.Create(new IdentityRole("Student"));
                }

                if (!roleManager.RoleExists("Assignor"))
                {
                    roleManager.Create(new IdentityRole("Assignor"));
                }

                if (!roleManager.RoleExists("FacilityHead"))
                {
                    roleManager.Create(new IdentityRole("FacilityHead"));
                }

            }
        }
    }
}