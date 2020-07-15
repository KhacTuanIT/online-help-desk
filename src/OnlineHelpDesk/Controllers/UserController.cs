using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineHelpDesk.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

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
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: User
        public async Task<ActionResult> Index(string user)
        {
            ApplicationUser appUser;
            if (string.IsNullOrEmpty(user) || user == "me")
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Login", "Account", new { returnUrl = "/me" });

                if (User.IsInRole("SuperAdmin"))
                    return RedirectToAction("Index", "Admin");

                return RedirectToRoute("Profile", new { user = User.Identity.GetUserName() });
            }
            else
            {
                appUser = await UserManager.FindByNameAsync(user);
                if (appUser == null)
                {
                    return HttpNotFound();
                }

                var profileUser = new ProfileViewModel()
                {
                    Email = appUser.Email,
                    FullName = appUser.FullName,
                    UserIdentity = appUser.UserIdentityCode,
                    Role = appUser.Roles.FirstOrDefault().ToString(),
                    Contact = appUser.Contact ?? "",
                    ProfilePicture = appUser.Avatar ?? ""
                };
                return View(profileUser);
            }
        }


        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = "ChangePasswordSuccess" });
            }
            //AddErrors(result);
            return View(model);
        }
    }
}
