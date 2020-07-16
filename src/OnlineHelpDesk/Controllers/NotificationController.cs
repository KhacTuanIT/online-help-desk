using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineHelpDesk.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
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

        // GET: Notification
        public ActionResult Get()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Configuration.ProxyCreationEnabled = false;
            string userId = User.Identity.GetUserId();
            List<Notification> listNotification = new List<Notification>();
            var notifications = (from n in context.Notifications
                                 where n.UserId == userId
                                 select n).ToList();

            Dictionary<string, Notification> dictNotifications = new Dictionary<string, Notification>();
            foreach (var notification in notifications)
            {
                if (notification.Seen == false)
                    dictNotifications.Add(notification.Id.ToString(), notification);
            }
            return Json(dictNotifications, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Khi chưa logout mà tắt server, drop database, rồi chạy lại.
                // Logout lại lần nữa sẽ bị null cái userId
                var userId = User.Identity.GetUserId();
                string rolename = UserManager.GetRoles(userId).FirstOrDefault();
                return rolename;
            }
            return "";
        }
    }
}