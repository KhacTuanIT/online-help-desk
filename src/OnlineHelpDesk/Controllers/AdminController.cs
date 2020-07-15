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

namespace OnlineHelpDesk.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

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

        // IMPORT DATA FROM EXCEL

        [HttpPost]
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
                                i++;
                                listProfile.Add(new ProfileViewModel
                                {
                                    FullName = worksheet.Cells[i, 2].Value?.ToString() + " " + worksheet.Cells[i, 3].Value?.ToString(),
                                    UserIdentity = worksheet.Cells[i, 4].Value?.ToString(),
                                    Email = worksheet.Cells[i, 5].Value?.ToString(),
                                    Contact = worksheet.Cells[i, 6].Value?.ToString(),
                                    Role = "Student"
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
                        Avatar = "~/Content/AppImages/useraccount.png",
                        Contact = p.Contact,
                        MustChangePassword = true,
                        CreatedAt = DateTime.Now
                    };
                    var result = UserManager.Create(newUser, "123@123a");
                    if (result.Succeeded)
                    {
                        UserManager.AddToRole(newUser.Id, "Student");
                    }
                })
            );
        }

        #region Helpers
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
        #endregion
    }
}