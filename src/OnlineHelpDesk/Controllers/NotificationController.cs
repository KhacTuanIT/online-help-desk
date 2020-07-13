using Microsoft.AspNet.Identity;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineHelpDesk.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        [Authorize]
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
    }
}