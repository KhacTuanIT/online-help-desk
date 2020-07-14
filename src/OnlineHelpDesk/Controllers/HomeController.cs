using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
            //if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
            int requestCount = context.Requests.Count();
            int userCount = context.Users.Count();
            int facilityCount = context.Facilities.Count();
            int equipmentCount = context.Equipments.Count();
            return View(new HomeViewModel() { 
                Notifications = GetNotifications(), 
                RequestViewModels = null,
                Requests = requestCount,
                Users = userCount,
                Facilities = facilityCount,
                Equipments = equipmentCount
            });
        }

        public ActionResult About()
        {
            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];

            return View();
        }

        public ActionResult Contact()
        {
            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];

            return View();
        }

        public ActionResult ListRequest()
        {
            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
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

            return View(new HomeViewModel() { Notifications = GetNotifications(), RequestViewModels = requestViewModels });
        }

        [HttpGet]
        public ActionResult CreateNewRequest()
        {
            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
            return View(new CreateNewRequestViewModel { Facilities = GetFacilities() });
        }

        [HttpPost]
        public ActionResult CreateNewRequest(NewRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.EquipmentId == 0)
                    TempData["Message"] = "Missing equipment field";
                return RedirectToAction("CreateNewRequest");
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var statusTypeId = context.StatusTypes.Where(x => x.TypeName == "Created").FirstOrDefault().Id;
                    var createdTime = DateTime.Now;
                    RequestStatus requestStatus = new RequestStatus()
                    {
                        StatusTypeId = statusTypeId,
                        TimeCreated = createdTime,
                        Message = "Created Request"
                    };

                    context.RequestStatus.Add(requestStatus);
                    context.SaveChanges();

                    var requestStatusId = requestStatus.Id;
                    var userId = User.Identity.GetUserId();
                    Request request = new Request()
                    {
                        PetitionerId = userId,
                        EquipmentId = model.EquipmentId,
                        Message = model.Message,
                        RequestStatusId = requestStatusId,
                        RequestTypeId = 3
                    };
                    context.Requests.Add(request);

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            TempData["Message"] += validationError.ErrorMessage + "<br>";
                        }
                    }
                    transaction.Rollback();
                    return View(new CreateNewRequestViewModel { Facilities = GetFacilities(), NewRequestViewModel = model });
                }
            }
            TempData["Message"] = "Create Request successfully!";
            return RedirectToAction("Index");
        }

        public List<Notification> GetNotifications()
        {
            string userId = User.Identity.GetUserId();
            return (from n in context.Notifications
                    where n.UserId == userId
                    select n).ToList();
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