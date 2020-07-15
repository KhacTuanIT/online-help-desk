using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            //var requestRecords = from r in context.Requests
            //                    join e in context.Equipments on r.EquipmentId equals e.Id into tb1
            //                    from e in tb1.ToList()
            //                    join f in context.Facilities on e.FacilityId equals f.Id
            //                    join et in context.EquipmentTypes on e.ArtifactId equals et.Id
            //                    join rs in context.RequestStatus on r.RequestStatusId equals rs.Id into tb2
            //                    from rs in tb2.ToList()
            //                    join st in context.StatusTypes on rs.StatusTypeId equals st.Id
            //                    join rt in context.RequestTypes on r.RequestTypeId equals rt.Id
            //                    join u in context.Users on r.PetitionerId equals u.Id
            //                    select new RequestViewModel
            //                    {
            //                        Id = r.Id,
            //                        Petitioner = u.UserName,
            //                        Equipment = et.TypeName,
            //                        Facility = f.Name,
            //                        RequestType = rt.TypeName,
            //                        RequestMessage = r.Message,
            //                        CreatedTime = rs.TimeCreated
            //                    };

            //List<RequestViewModel> requestViewModels = requestRecords.ToList();

            //return View(new HomeViewModel() { Notifications = notifications, RequestViewModels = requestViewModels });

            return View();
        }

        [HttpGet]
        public ActionResult CreateNewRequest()
        {
            return View(new CreateNewRequestViewModel { Facilities = GetFacilities() });
        }

        [HttpPost]
        public ActionResult CreateNewRequest(NewRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new CreateNewRequestViewModel { Facilities = GetFacilities(), NewRequestViewModel = model });
            }

            //using (var transaction = context.Database.BeginTransaction())
            //{
            //    try
            //    {
            //        string userId = User.Identity.GetUserId();
            //        Request request = new Request()
            //        {
            //            PetitionerId = userId,
            //            EquipmentId = model.EquipmentId,
            //            Message = model.Message,
            //            RequestTypeId = 3
            //        };

            //        context.Requests.Add(request);
            //        context.SaveChanges();

            //        int requestId = request.Id;
            //        RequestStatus requestStatus = new RequestStatus()
            //        {
            //            RequestId = requestId,
            //            StatusTypeId = 1,
            //            TimeCreated = DateTime.Now
            //        };

            //        context.RequestStatus.Add(requestStatus);
            //        context.SaveChanges();
            //        int requestStatusId = requestStatus.Id;

            //        request.RequestStatusId = requestStatusId;

            //        context.Entry(request).State = EntityState.Modified;
            //        context.SaveChanges();

            //        ViewBag.Message = "Create Request successfully!";
            //    }
            //    catch (Exception e)
            //    {
            //        ViewBag.Message = e.Message;
            //        transaction.Rollback();
            //        return View(new CreateNewRequestViewModel { Facilities = GetFacilities(), NewRequestViewModel = model });
            //    }
            //}

            return RedirectToAction("Index");
        }

        public List<Facility> GetFacilities()
        {
            return (from f in context.Facilities
                    select f).ToList();
        }

        public ActionResult GetEquipment(int id)
        {
            var equipmentTypes = (from et in context.EquipmentTypes
                                  select et).ToList();
            Dictionary<string, string> dictEquipmentType = new Dictionary<string, string>();
            foreach (var et in equipmentTypes)
            {
                dictEquipmentType.Add(et.Id.ToString(), et.TypeName);
            }
            var equipts = (from e in context.Equipments
                           where e.FacilityId == id
                           select e).ToList();

            Dictionary<string, string> dictEquipment = new Dictionary<string, string>();
            foreach (var e in equipts)
            {
                dictEquipment.Add(e.Id.ToString(), dictEquipmentType[e.ArtifactId.ToString()]);
            }
            return Json(dictEquipment, JsonRequestBehavior.AllowGet);
        }
    }
}