using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineHelpDesk.Services
{
    public class UserService : IDisposable
    {
        private ApplicationDbContext db;
        public UserService(ApplicationDbContext db) => this.db = db;

        public bool CreateUser(ApplicationUser user, string role, string password = null)
        {
            user.MustChangePassword = true;
            user.CreatedAt = DateTime.UtcNow;
            user.Avatar = user.Avatar ?? AppInfo.DefaultProfilePicture;
            var result = UserManager.Create(user, password ?? AppInfo.DefaultUserPassword);
            if (result.Succeeded)
            {
                result = UserManager.AddToRole(user.Id, role);

                if (role.Equals("FacilityHead", StringComparison.OrdinalIgnoreCase))
                {
                    db.FacilityHeads.Add(new FacilityHead
                    {
                        UserId = user.Id,
                        PositionName = "New Head",
                    });
                    db.SaveChanges();
                }
            }
            return result.Succeeded;
        }

        public bool CreateUser(ApplicationUser user)
        {
            return CreateUser(user, "Student");
        }

        public bool v(ProfileViewModel user)
        {
            var result = CreateUser(new ApplicationUser
            {
                UserName = user.UserIdentity ?? "unknown",
                FullName = user.FullName ?? "unknown",
                Avatar = user.ProfilePicture ?? AppInfo.DefaultProfilePicture,
                Email = user.Email ?? "unknown",
                Contact = user.Contact ?? ""
            }, role: user.Role);
            return result;
        }

        #region Helpers
        private UserManager<ApplicationUser> _userManager;
        public UserManager<ApplicationUser> UserManager
        {
            get => _userManager ?? new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            private set => _userManager = value;
        }

        private RoleManager<IdentityRole> _roleManager;
        public RoleManager<IdentityRole> RoleManager
        {
            get => _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            private set => _roleManager = value;
        }

        public void Dispose() => Dispose(true);
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager?.Dispose();
                _userManager = null;

                _roleManager?.Dispose();
                _roleManager = null;

                // coalescing statement in C# 8.0 :))
                //db ??= null;
                this.db = null;
            }
        }
        #endregion
    }
}