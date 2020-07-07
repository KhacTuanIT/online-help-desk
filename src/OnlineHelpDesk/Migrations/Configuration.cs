namespace OnlineHelpDesk.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OnlineHelpDesk.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineHelpDesk.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OnlineHelpDesk.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //InitializeIdentityForEF(context);
            //base.Seed(context);

        }
        //private void InitializeIdentityForEF(ApplicationDbContext context)
        //{
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        //    var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    var myinfo = new ApplicationUser()
        //    {
        //        UserName = "admin",
        //        Email = "admin@ohd.com",
        //        FullName = "Super Admin"
        //    };

        //    string saPassword = "ADM@123a";

        //    //Create Role Test and User Test
        //    RoleManager.Create(new IdentityRole(test));
        //    UserManager.Create(new ApplicationUser() { UserName = test });

        //    //Create Role Admin if it does not exist
        //    if (!RoleManager.RoleExists(name))
        //    {
        //        var roleresult = RoleManager.Create(new IdentityRole(name));
        //    }

        //    //Create User=Admin with password=123456
        //    var user = new ApplicationUser();
        //    user.UserName = name;
        //    user.HomeTown = "Seattle";
        //    user.MyUserInfo = myinfo;
        //    var adminresult = UserManager.Create(user, password);

        //    //Add User Admin to Role Admin
        //    if (adminresult.Succeeded)
        //    {
        //        var result = UserManager.AddToRole(user.Id, name);
        //    }
        //}
    }
}
