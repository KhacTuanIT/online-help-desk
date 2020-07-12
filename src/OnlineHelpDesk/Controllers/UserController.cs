using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Index(string user)
        {
            ApplicationUser appUser;
            if (string.IsNullOrEmpty(user) || user == "me")
            {
                string userName = User.Identity.Name;
                appUser = UserManager.FindByName(userName);

                var profileUser = new ProfileViewModel()
                {
                    Email = appUser.Email,
                    FullName = appUser.FullName,
                    UserIdentity = appUser.UserIdentityCode,
                    Role = appUser.Roles.FirstOrDefault().ToString(),
                    Address = appUser.Address,
                    ProfilePicture = appUser.Avatar ?? null
                };

                return View(profileUser);
            }
            else if (!User.IsInRole("SuperAdmin") && user != "admin")
            {
                appUser = UserManager.FindByName(user);
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
                    Address = appUser.Address,
                    ProfilePicture = appUser.Avatar ?? null
                };
                return View(appUser);
            }

            return HttpNotFound();
        }

        // GET: User
        public ActionResult List()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
