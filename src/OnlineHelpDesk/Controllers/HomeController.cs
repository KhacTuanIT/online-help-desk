using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineHelpDesk.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            List<Notification> notifications = (from n in context.Notifications
                                                where n.UserId == userId
                                                select n).ToList();
            int requestCount = context.Requests.Count();
            int userCount = context.Users.Count();
            int facilityCount = context.Facilities.Count();
            int equipmentCount = context.Equipments.Count();
            return View(new HomeViewModel() { 
                Notifications = notifications, 
                RequestViewModels = null,
                Requests = requestCount,
                Users = userCount,
                Facilities = facilityCount,
                Equipments = equipmentCount
            });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListRequest()
        {
            string userId = User.Identity.GetUserId();
            List <Notification> notifications = (from n in context.Notifications
                                                where n.UserId == userId
                                                select n).ToList();
            var requestRecords = from r in context.Requests
                                join e in context.Equipments on r.EquipmentId equals e.Id into tb1
                                from e in tb1.ToList()
                                join f in context.Facilities on e.FacilityId equals f.Id
                                join et in context.EquipmentTypes on e.ArtifactId equals et.Id
                                join rs in context.RequestStatus on r.RequestStatusId equals rs.Id into tb2
                                from rs in tb2.ToList()
                                join st in context.StatusTypes on rs.StatusTypeId equals st.Id
                                join rt in context.RequestTypes on r.RequestTypeId equals rt.Id
                                join u in context.Users on r.PetitionerId equals u.Id
                                select new RequestViewModel
                                {
                                    Id = r.Id,
                                    Petitioner = u.UserName,
                                    Equipment = et.TypeName,
                                    Facility = f.Name,
                                    RequestType = rt.TypeName,
                                    RequestMessage = r.Message,
                                    CreatedTime = rs.TimeCreated
                                };

            List<RequestViewModel> requestViewModels = requestRecords.ToList();

            return View(new HomeViewModel() { Notifications = notifications, RequestViewModels = requestViewModels });
        }

        public ActionResult CreateNewRequest()
        {
            var userId = User.Identity.GetUserId();
            List<Notification> notifications = (from n in context.Notifications
                                                where n.UserId == userId
                                                select n).ToList();
            return View(new HomeViewModel() { Notifications = notifications, RequestViewModels = null });
        }
    }
}