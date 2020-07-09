using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineHelpDesk.Models;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity.Owin;

namespace OnlineHelpDesk.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            var users = await db.Users.ToListAsync();
            return View(users);
                //await Details("me");  
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(string userId)
        {
            if (userId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ApplicationUser appUser = null;

            if (Regex.IsMatch(userId, @"^w{2},\d{6}", RegexOptions.IgnoreCase))
            {
                // Get user by IdentityCode
                // Check regex for student identity code: DE130032
                //appUser = UserManager.FindByEmailAsync("abcbc");
                appUser = await db.Users
                        .Where(u => u.UserIdentityCode.Equals(userId, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefaultAsync();
            }
            else
            {
                if (userId.Equals("me", StringComparison.OrdinalIgnoreCase))
                {
                    if (Request.IsAuthenticated)
                        // show personal profile if logged in then 
                        userId = Membership.GetUser().UserName;
                    else
                        return RedirectToAction("Index", "Home");
                }

                appUser = await db.Users
                        .Where(u => u.UserName.Equals(userId, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefaultAsync();
            }

            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
