using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Web.DynamicData;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineHelpDesk.Services;

namespace OnlineHelpDesk.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Import(string importFor)
        {
            ViewBag.ImportFor = importFor;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadSampleData()
        {
            DatabaseHelper.SeedData();
            ViewBag.Message = "Sample data loaded";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ViewImported(List<ProfileViewModel> model)
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Users(string role = "Student")
        {
            var usersIds = RoleManager.FindByName(role).Users.Select(u => u.UserId).ToList();
            if (usersIds != null)
            {
                List<ProfileViewModel> model = new List<ProfileViewModel>();
                usersIds.ForEach(uid =>
                {
                    var u = UserManager.FindById(uid);
                    model.Add(new ProfileViewModel
                    {
                        FullName = u.FullName ?? "unknown",
                        Contact = u.Contact ?? "unknown",
                        Email = u.Email ?? "unknown",
                        ProfilePicture = u.Avatar ?? "unknown",
                        UserIdentity = u.UserIdentityCode ?? "unknown"
                    });
                });
                ViewBag.Role = role;
                return View(model);
            }

            ViewBag.Message = "Role not found";
            return View();
        }

        // IMPORT DATA FROM EXCEL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadExcel(ImportDataViewModel model, string role = "Student")
        {
            if (ModelState.IsValid)
            {
                try
                {

                    // save to file
                    //var file = model.File;
                    //string filename = Path.GetFileName(file.FileName);
                    //string path = Path.Combine(Server.MapPath("~/Content/Uploaded"), filename);
                    //file.SaveAs(path);

                    var file = model.File;

                    if (file == null || file.ContentLength <= 0)
                        throw new Exception("[File Error] Invaid Excel file");
                    if (!".xlsx".Contains(Path.GetExtension(file.FileName)))
                        throw new Exception("[File Error] Invaid Excel file");

                    if (db.Roles.FirstOrDefault(r => r.Name == role) == null) throw new Exception("[ERROR] Wrong role");

                    List<ProfileViewModel> listProfile = new List<ProfileViewModel>();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        model.File.InputStream.CopyTo(stream);
                        // Excel license
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0];

                            int i = worksheet.Dimension.Start.Row + 1;

                            while (int.TryParse((worksheet.Cells[++i, 1].Value ?? "").ToString(), out _))
                            {
                                listProfile.Add(new ProfileViewModel
                                {
                                    FullName = worksheet.Cells[i, 2].Value?.ToString() + " " + worksheet.Cells[i, 3].Value?.ToString(),
                                    UserIdentity = worksheet.Cells[i, 4].Value?.ToString(),
                                    Email = worksheet.Cells[i, 5].Value?.ToString(),
                                    Contact = worksheet.Cells[i, 6].Value?.ToString(),
                                    Role = role
                                });
                            }

                        }
                    }

                    await ProfileModels2Database(listProfile);

                    return View("ViewImported", listProfile);
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    return View("Import");
                }
            }

            return View("Index");
        }

        private async Task ProfileModels2Database(List<ProfileViewModel> listProfile)
        {
            await Task.Run(() =>
                listProfile.ForEach((p) =>
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = p.UserIdentity,
                        Email = p.Email,
                        FullName = p.FullName,
                        Avatar = AppInfo.DefaultProfilePicture,
                        Contact = p.Contact,
                        MustChangePassword = true,
                        CreatedAt = DateTime.Now
                    };
                    var result = UserManager.Create(newUser, "123@123a");
                    if (result.Succeeded)
                    {
                        UserManager.AddToRole(newUser.Id, p.Role);
                        if (p.Role == "FacilityHead")
                        {
                            db.FacilityHeads.Add(new FacilityHead
                            {
                                UserId = newUser.Id
                            });
                            db.SaveChangesAsync();
                        }
                    }
                })
            );
        }

        #region Helpers
        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _roleManager;

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
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion
    }
}