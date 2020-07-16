using Microsoft.Ajax.Utilities;
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
            List<RequestViewModel> requestViewModels = new List<RequestViewModel>();
            var statusRequestId = context.StatusTypes.Where(x => x.TypeName == "Created").FirstOrDefault().Id;
            if (User.IsInRole("FacilityHead") || User.IsInRole("SuperAdmin"))
            {
                requestViewModels = (from r in context.Requests
                                     join e in context.Equipments on r.EquipmentId equals e.Id into tb1
                                     from e in tb1.ToList()
                                     join f in context.Facilities on e.FacilityId equals f.Id
                                     join et in context.EquipmentTypes on e.ArtifactId equals et.Id
                                     join rs in context.RequestStatus on r.Id equals rs.RequestId into tb2 // Chỗ ni lỗi do xóa bên models
                                     from rs in tb2.ToList()
                                     join st in context.StatusTypes on rs.StatusTypeId equals st.Id
                                     join rt in context.RequestTypes on r.RequestTypeId equals rt.Id
                                     join u in context.Users on r.PetitionerId equals u.Id
                                     orderby r.PetitionerId
                                     where st.Id == statusRequestId
                                     select new RequestViewModel
                                     {
                                         Id = r.Id,
                                         Petitioner = u.FullName,
                                         Equipment = et.TypeName,
                                         Facility = f.Name,
                                         RequestType = rt.TypeName,
                                         RequestMessage = r.Message,
                                         CreatedTime = rs.TimeCreated
                                     }).ToList();
            }
            
            if (User.IsInRole("Assignor"))
            {
                string userId = User.Identity.GetUserId();
                int assignedHeadId = context.FacilityHeads.Where(f => f.UserId == userId).FirstOrDefault().Id;
                requestViewModels = (from r in context.Requests
                                     join e in context.Equipments on r.EquipmentId equals e.Id into tb1
                                     from e in tb1.ToList()
                                     join f in context.Facilities on e.FacilityId equals f.Id
                                     join et in context.EquipmentTypes on e.ArtifactId equals et.Id
                                     join rs in context.RequestStatus on r.Id equals rs.RequestId into tb2 // Chỗ ni cũng vậy
                                     from rs in tb2.ToList()
                                     join st in context.StatusTypes on rs.StatusTypeId equals st.Id
                                     join rt in context.RequestTypes on r.RequestTypeId equals rt.Id
                                     join u in context.Users on r.PetitionerId equals u.Id
                                     where r.AssignedHeadId == assignedHeadId
                                     orderby r.PetitionerId
                                     where st.Id == statusRequestId
                                     select new RequestViewModel
                                     {
                                         Id = r.Id,
                                         Petitioner = u.FullName,
                                         Equipment = et.TypeName,
                                         Facility = f.Name,
                                         RequestType = rt.TypeName,
                                         RequestMessage = r.Message,
                                         CreatedTime = rs.TimeCreated
                                     }).ToList();
            }

            return View(new HomeViewModel() { Notifications = GetNotifications(), RequestViewModels = requestViewModels });
        }

        [HttpGet]
        public ActionResult CreateNewRequest()
        {
            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
            return View(new CreateNewRequestViewModel { Facilities = GetFacilities(), RequestTypes = GetRequestTypes() });
        }

        [HttpPost]
        public ActionResult CreateNewRequest(NewRequestViewModel model)
        {
            if (model.EquipmentId == 0)
            {
                TempData["Message"] += "Missing equipment field";
                return RedirectToAction("CreateNewRequest");
            }
            if (model.RequestTypeId == 0)
            {
                TempData["Message"] += "Missing Request type field";
                return RedirectToAction("CreateNewRequest");
            }
                
            if (!ModelState.IsValid)
            {
                if (model.EquipmentId == 0)
                    TempData["Message"] += "Missing equipment field \n";
                if (model.RequestTypeId == 0)
                    TempData["Message"] += "Missing Request type field";
                return RedirectToAction("CreateNewRequest");
            }

            var adminId = RoleManager.FindByName("SuperAdmin").Users.FirstOrDefault().UserId;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var statusTypeId = context.StatusTypes.Where(x => x.TypeName == "Created").FirstOrDefault().Id;
                    var createdTime = DateTime.Now;
                    string message = "";
                    if (model.Message == null || model.Message.Trim() == "")
                    {
                        message = "Request";
                    }
                    else
                    {
                        message = model.Message;
                    }
                    var userId = User.Identity.GetUserId();
                    Request request = new Request()
                    {
                        PetitionerId = userId,
                        EquipmentId = model.EquipmentId,
                        Message = message,
                        RequestTypeId = model.RequestTypeId
                    };
                    context.Requests.Add(request);
                    context.SaveChanges();


                    var requestId = request.Id;
                    RequestStatus requestStatus = new RequestStatus()
                    {
                        RequestId = requestId,
                        StatusTypeId = statusTypeId,
                        TimeCreated = createdTime,
                        Message = "Created Request"
                    };

                    context.RequestStatus.Add(requestStatus);
                    context.SaveChanges();
                    
                    string petitioner = User.Identity.GetUserName();
                    var facilityHeads = (from fh in context.FacilityHeads
                                         select fh);
                    var requestType = context.RequestTypes.Where(x => x.Id == model.RequestTypeId).FirstOrDefault().TypeName;
                    foreach (var item in facilityHeads)
                    {
                        if (item.UserId != userId)
                        {
                            context.Notifications.Add(new Notification()
                            {
                                UserId = item.UserId,
                                Message = petitioner + " has been created a " + requestType + " request",
                                Seen = false,
                                CreatedAt = createdTime
                            });
                        } 
                    }
                    context.Notifications.Add(new Notification()
                    {
                        UserId = userId,
                        Message = petitioner + " has been created a " + requestType + " request",
                        Seen = false,
                        CreatedAt = createdTime
                    });

                    context.Notifications.Add(new Notification()
                    {
                        UserId = adminId,
                        Message = petitioner + " has been created a " + requestType + " request",
                        Seen = false,
                        CreatedAt = createdTime
                    });
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (DbEntityValidationException)
                {
                    TempData["Message"] += "Missing field";
                    transaction.Rollback();
                    return RedirectToAction("CreateNewRequest");
                }
            }
            TempData["Message"] = "Create Request successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reply(HandleViewModel model)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                string userName = User.Identity.GetUserName();
                string statusMessage = "";
                var adminId = RoleManager.FindByName("SuperAdmin").Users.FirstOrDefault().UserId;
                DateTime createdTime = DateTime.Now;
                var statusTypeId = context.StatusTypes.Where(x => x.TypeName == "Reply").FirstOrDefault().Id;
                if (model.StatusMessage == "" || model.StatusMessage == null)
                {
                    statusMessage = "Your request is reply";
                }
                else
                {
                    statusMessage = model.StatusMessage;
                }
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        RequestStatus requestStatus = new RequestStatus()
                        {
                            RequestId = model.RequestId,
                            StatusTypeId = statusTypeId,
                            Message = statusMessage,
                            TimeCreated = createdTime
                        };
                        context.RequestStatus.Add(requestStatus);
                        context.SaveChanges();

                        var request = context.Requests.Where(x => x.Id == model.RequestId).FirstOrDefault();

                        if (request.PetitionerId != null)
                        {
                            Notification notification = new Notification()
                            {
                                UserId = request.PetitionerId,
                                Message = "Your request '" + request.Message + "' has been replied!",
                                Seen = false,
                                CreatedAt = createdTime
                            };
                            context.Notifications.Add(notification);
                            notification = new Notification()
                            {
                                UserId = adminId,
                                Message = "Request '" + request.Message + "' has been replied by " + userName,
                                Seen = false,
                                CreatedAt = createdTime
                            };
                            context.Notifications.Add(notification);
                            context.SaveChanges();
                        }
                        if (request.AssignedHeadId != null)
                        {
                            var assginedHeadUserId = context.FacilityHeads.Where(x => x.Id == request.AssignedHeadId).FirstOrDefault().UserId;
                            Notification notification = new Notification()
                            {
                                UserId = assginedHeadUserId,
                                Message = "Your assigned request '" + request.Message + "' has been replied by " + userName + "!",
                                Seen = false,
                                CreatedAt = createdTime
                            };
                            context.Notifications.Add(notification);
                            context.SaveChanges();
                            if (assginedHeadUserId != userId)
                            {
                                notification = new Notification()
                                {
                                    UserId = assginedHeadUserId,
                                    Message = "You was reply for request '" + request.Message + "!",
                                    Seen = false,
                                    CreatedAt = createdTime
                                };
                                context.Notifications.Add(notification);
                                context.SaveChanges();
                            }
                            notification = new Notification()
                            {
                                UserId = adminId,
                                Message = "Request '"+ request.Message +"' has been replied by " + userName,
                                Seen = false,
                                CreatedAt = createdTime
                            };
                            context.Notifications.Add(notification);
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        TempData["Message"] = "Has an error when reply request!";
                        return RedirectToAction("ListRequest");
                    }
                }
                TempData["Message"] = "Reply for " + userName + "'s request successfully!";
                return RedirectToAction("ListRequest");
            }
            catch (Exception)
            {
                TempData["Message"] = "Has an error when reply request!";
                return RedirectToAction("ListRequest");
            }
            
        }

        [HttpPost]
        public ActionResult Assign(AssignViewModel model)
        {
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.GetUserName();
            string statusMessage = "";
            var adminId = RoleManager.FindByName("SuperAdmin").Users.FirstOrDefault().UserId;
            DateTime createdTime = DateTime.Now;
            var statusTypeId = context.StatusTypes.Where(x => x.TypeName == "Assigned").FirstOrDefault().Id; 
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Missing field when assign";
                return RedirectToAction("ListRequest");
            }
            if (model.StatusMessage == "" || model.StatusMessage == null)
            {
                statusMessage = "Your request is assigned";
            }
            else
            {
                statusMessage = model.StatusMessage;
            }
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Request request = context.Requests.Where(x => x.Id == model.RequestId).FirstOrDefault();
                    request.AssignedHeadId = model.AssignedHeadId;
                    context.Entry(request).State = EntityState.Modified;

                    RequestStatus requestStatus = new RequestStatus()
                    {
                        RequestId = request.Id,
                        StatusTypeId = statusTypeId,
                        Message = statusMessage,
                        TimeCreated = createdTime
                    };
                    context.RequestStatus.Add(requestStatus);
                    context.SaveChanges();

                    if (request.PetitionerId != null)
                    {
                        Notification notification = new Notification()
                        {
                            UserId = request.PetitionerId,
                            Message = "Your request '" + request.Message + "' has been assigned!",
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        notification = new Notification()
                        {
                            UserId = adminId,
                            Message = "Request '" + request.Message + "' has been assigned by " + userName,
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }
                    if (request.AssignedHeadId != null)
                    {
                        var assginedHeadUserId = context.FacilityHeads.Where(x => x.Id == request.AssignedHeadId).FirstOrDefault().UserId;
                        Notification notification = new Notification()
                        {
                            UserId = assginedHeadUserId,
                            Message = "Your is assigned '" + request.Message + "' request!",
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                        if (assginedHeadUserId != userId)
                        {
                            notification = new Notification()
                            {
                                UserId = assginedHeadUserId,
                                Message = "You was assigned for request '" + request.Message + "!",
                                Seen = false,
                                CreatedAt = createdTime
                            };
                            context.Notifications.Add(notification);
                            context.SaveChanges();
                        }
                        notification = new Notification()
                        {
                            UserId = adminId,
                            Message = "Request '" + request.Message + "' has been assigned by " + userName,
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    TempData["Message"] = "Has an error when assign request!";
                    return RedirectToAction("ListRequest");
                }
            }
            TempData["Message"] = "Assign for " + userName + "'s request successfully!";
            return RedirectToAction("ListRequest");
        }

        [HttpPost]
        public ActionResult Refuse(HandleViewModel model)
        {
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.GetUserName();
            string statusMessage = "";
            var adminId = RoleManager.FindByName("SuperAdmin").Users.FirstOrDefault().UserId;
            DateTime createdTime = DateTime.Now;
            var statusTypeId = context.StatusTypes.Where(x => x.TypeName == "Closed").FirstOrDefault().Id;
            if (model.StatusMessage == "" || model.StatusMessage == null)
            {
                statusMessage = "Your request is refuse";
            }
            else
            {
                statusMessage = model.StatusMessage;
            }
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    RequestStatus requestStatus = new RequestStatus()
                    {
                        RequestId = model.RequestId,
                        StatusTypeId = statusTypeId,
                        Message = statusMessage,
                        TimeCreated = createdTime
                    };
                    context.RequestStatus.Add(requestStatus);
                    context.SaveChanges();

                    var request = context.Requests.Where(x => x.Id == model.RequestId).FirstOrDefault();

                    if (request.PetitionerId != null)
                    {
                        Notification notification = new Notification()
                        {
                            UserId = request.PetitionerId,
                            Message = "Your request '" + request.Message + "' has been refuse!",
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        notification = new Notification()
                        {
                            UserId = adminId,
                            Message = "Request '" + request.Message + "' has been refuse by " + userName,
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }
                    if (request.AssignedHeadId != null)
                    {
                        var assginedHeadUserId = context.FacilityHeads.Where(x => x.Id == request.AssignedHeadId).FirstOrDefault().UserId;
                        Notification notification = new Notification()
                        {
                            UserId = assginedHeadUserId,
                            Message = "You are refused a request '" + request.Message + "!",
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                        if (assginedHeadUserId != userId)
                        {
                            notification = new Notification()
                            {
                                UserId = assginedHeadUserId,
                                Message = "'" + request.Message + " has been refuse by " + userName + "!",
                                Seen = false,
                                CreatedAt = createdTime
                            };
                            context.Notifications.Add(notification);
                            context.SaveChanges();
                        }
                        notification = new Notification()
                        {
                            UserId = adminId,
                            Message = "Request '" + request.Message + "' has been refuse by " + userName,
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    TempData["Message"] = "Has an error when refuse request!";
                    return RedirectToAction("ListRequest");
                }
            }
            TempData["Message"] = "Reply for " + userName + "'s request successfully!";
            return RedirectToAction("ListRequest");
        }

        [HttpPost]
        public ActionResult Done(HandleViewModel model)
        {
            string userId = User.Identity.GetUserId();
            string userName = User.Identity.GetUserName();
            string statusMessage = "";
            var adminId = RoleManager.FindByName("SuperAdmin").Users.FirstOrDefault().UserId;
            DateTime createdTime = DateTime.Now;
            var statusTypeId = context.StatusTypes.Where(x => x.TypeName == "Completed").FirstOrDefault().Id;
            if (model.StatusMessage == "" || model.StatusMessage == null)
            {
                statusMessage = "Your request is done";
            }
            else
            {
                statusMessage = model.StatusMessage;
            }
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    RequestStatus requestStatus = new RequestStatus()
                    {
                        RequestId = model.RequestId,
                        StatusTypeId = statusTypeId,
                        Message = statusMessage,
                        TimeCreated = createdTime
                    };
                    context.RequestStatus.Add(requestStatus);
                    context.SaveChanges();

                    var request = context.Requests.Where(x => x.Id == model.RequestId).FirstOrDefault();

                    if (request.PetitionerId != null)
                    {
                        Notification notification = new Notification()
                        {
                            UserId = request.PetitionerId,
                            Message = "Your request '" + request.Message + "' has been done!",
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification); 
                        notification = new Notification()
                        {
                            UserId = adminId,
                            Message = "Request '" + request.Message + "' has been completed by " + userName,
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges(); 
                        
                    }
                    if (request.AssignedHeadId != null)
                    {
                        var assginedHeadUserId = context.FacilityHeads.Where(x => x.Id == request.AssignedHeadId).FirstOrDefault().UserId;
                        Notification notification = new Notification()
                        {
                            UserId = assginedHeadUserId,
                            Message = "You are done a request '" + request.Message + "!",
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                        if (assginedHeadUserId != userId)
                        {
                            notification = new Notification()
                            {
                                UserId = assginedHeadUserId,
                                Message = "'" + request.Message + " has been done by " + userName + "!",
                                Seen = false,
                                CreatedAt = createdTime
                            };
                            context.Notifications.Add(notification);
                            context.SaveChanges();
                        }
                        notification = new Notification()
                        {
                            UserId = adminId,
                            Message = "Request '" + request.Message + "' has been completed by " + userName,
                            Seen = false,
                            CreatedAt = createdTime
                        };
                        context.Notifications.Add(notification);
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    TempData["Message"] = "Has an error when reply request!";
                    return RedirectToAction("ListRequest");
                }
            }
            TempData["Message"] = "Reply for " + userName + "'s request successfully!";
            return RedirectToAction("ListRequest");
        }

        [HttpGet]
        public ActionResult YourRequest()
        {
            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
            string userId = User.Identity.GetUserId();
            var statusRequestId = context.StatusTypes.Where(x => x.TypeName == "Created").FirstOrDefault().Id;
            List<RequestViewModel> requestViewModels = new List<RequestViewModel>();
            
            if (!User.IsInRole("Assignor"))
            {
                requestViewModels = (from r in context.Requests
                                     join e in context.Equipments on r.EquipmentId equals e.Id into tb1
                                     from e in tb1.ToList()
                                     join f in context.Facilities on e.FacilityId equals f.Id
                                     join et in context.EquipmentTypes on e.ArtifactId equals et.Id
                                     join rs in context.RequestStatus on r.Id equals rs.RequestId into tb2
                                     from rs in tb2.ToList()
                                     join st in context.StatusTypes on rs.StatusTypeId equals st.Id
                                     join rt in context.RequestTypes on r.RequestTypeId equals rt.Id
                                     join u in context.Users on r.PetitionerId equals u.Id
                                     where r.PetitionerId == userId
                                     select new RequestViewModel
                                     {
                                         Id = r.Id,
                                         Petitioner = u.UserName,
                                         Equipment = et.TypeName,
                                         Facility = f.Name,
                                         RequestType = rt.TypeName,
                                         RequestMessage = r.Message,
                                         CreatedTime = rs.TimeCreated
                                     }).ToList();
            }
            else
            {
                var assignedHeadId = context.FacilityHeads.Where(x => x.UserId == userId).FirstOrDefault().Id;
                requestViewModels = (from r in context.Requests
                                     join e in context.Equipments on r.EquipmentId equals e.Id into tb1
                                     from e in tb1.ToList()
                                     join f in context.Facilities on e.FacilityId equals f.Id
                                     join et in context.EquipmentTypes on e.ArtifactId equals et.Id
                                     join rs in context.RequestStatus on r.Id equals rs.RequestId into tb2
                                     from rs in tb2.ToList()
                                     join st in context.StatusTypes on rs.StatusTypeId equals st.Id
                                     join rt in context.RequestTypes on r.RequestTypeId equals rt.Id
                                     join u in context.Users on r.PetitionerId equals u.Id
                                     where r.AssignedHeadId == assignedHeadId && st.Id != statusRequestId
                                     select new RequestViewModel
                                     {
                                         Id = r.Id,
                                         Petitioner = u.UserName,
                                         Equipment = et.TypeName,
                                         Facility = f.Name,
                                         RequestType = rt.TypeName,
                                         RequestMessage = r.Message,
                                         CreatedTime = rs.TimeCreated
                                     }).ToList();
            }

            return View(new HomeViewModel() { Notifications = GetNotifications(), RequestViewModels = requestViewModels });
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

        public List<RequestType> GetRequestTypes()
        {
            return (from rt in context.RequestTypes
                    select rt).ToList();
        }

        public ActionResult GetFacilityHeadAssigned()
        {
            try
            {
                var users = (from u in context.Users
                             select u).ToList();
                Dictionary<string, string> dictUser = new Dictionary<string, string>();
                foreach (var user in users)
                {
                    dictUser.Add(user.Id, user.FullName);
                }

                var facilityHeads = (from fh in context.FacilityHeads
                                     select fh).ToList();
                Dictionary<string, string> dictFacilityHeads = new Dictionary<string, string>();
                foreach (var facilityHead in facilityHeads)
                {
                    dictFacilityHeads.Add(facilityHead.Id.ToString(), dictUser[facilityHead.UserId]);
                }
                return Json(dictFacilityHeads, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public ActionResult GetResponse(int id)
        {
            try
            {
                var assignedHeadId = context.Requests.Where(x => x.Id == id).FirstOrDefault().AssignedHeadId;
                var statusTypeId = context.StatusTypes.Where(x => x.TypeName == "Created").FirstOrDefault().Id;

                List<ResponseViewModel> responseViewModels = new List<ResponseViewModel>();
                if (assignedHeadId != null) {
                    responseViewModels = (from rs in context.RequestStatus
                                          join st in context.StatusTypes on rs.StatusTypeId equals st.Id
                                          join r in context.Requests on rs.RequestId equals r.Id into tb1
                                          from r in tb1.ToList()
                                          join fh in context.FacilityHeads on r.AssignedHeadId equals fh.Id into tb2
                                          from fh in tb2.ToList()
                                          join u in context.Users on fh.UserId equals u.Id
                                          where rs.RequestId == id && st.Id != statusTypeId
                                          select new ResponseViewModel()
                                          {
                                              Id = rs.Id,
                                              AssignedHead = u.FullName,
                                              RequestType = st.TypeName,
                                              StatusMessage = rs.Message,
                                              CreatedTime = rs.TimeCreated.ToString()
                                          }).ToList();
                }
                else
                {
                    responseViewModels = (from rs in context.RequestStatus
                                          join st in context.StatusTypes on rs.StatusTypeId equals st.Id
                                          join r in context.Requests on rs.RequestId equals r.Id
                                          where rs.RequestId == id && st.Id != statusTypeId
                                          select new ResponseViewModel()
                                          {
                                              Id = rs.Id,
                                              AssignedHead = "",
                                              RequestType = st.TypeName,
                                              StatusMessage = rs.Message,
                                              CreatedTime = rs.TimeCreated.ToString()
                                          }).ToList();
                }
                
                Dictionary<string, ResponseViewModel> dictResponse = new Dictionary<string, ResponseViewModel>();
                foreach (var response in responseViewModels)
                {
                    dictResponse.Add(response.Id.ToString(), response);
                }
                return Json(dictResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        } 

        public ActionResult GetCal()
        {
            try
            {
                var createdId = context.StatusTypes.Where(x => x.TypeName == "Created").FirstOrDefault().Id;
                var created = context.RequestStatus.Where(x => x.StatusTypeId == createdId).Count();

                var assignedId = context.StatusTypes.Where(x => x.TypeName == "Assigned").FirstOrDefault().Id;
                var assigned = context.RequestStatus.Where(x => x.StatusTypeId == assignedId).Count();

                var processingId = context.StatusTypes.Where(x => x.TypeName == "Processing").FirstOrDefault().Id;
                var processing = context.RequestStatus.Where(x => x.StatusTypeId == processingId).Count();

                var completedId = context.StatusTypes.Where(x => x.TypeName == "Completed").FirstOrDefault().Id;
                var completed = context.RequestStatus.Where(x => x.StatusTypeId == completedId).Count();

                var closedId = context.StatusTypes.Where(x => x.TypeName == "Closed").FirstOrDefault().Id;
                var closed = context.RequestStatus.Where(x => x.StatusTypeId == closedId).Count();

                Dictionary<string, CalViewModel> dictCal = new Dictionary<string, CalViewModel>();
                CalViewModel calViewModel = new CalViewModel()
                {
                    Created = created,
                    Assigned = assigned,
                    Processing = processing,
                    Completed = completed,
                    Closed = closed
                };
                dictCal.Add("key", calViewModel);
                return Json(dictCal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult GetEquipment(int id)
        {
            try
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
            catch (Exception)
            {
                return null;
            }
        }
        #region Helpers
        private RoleManager<IdentityRole> _roleManager;
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

    }

}