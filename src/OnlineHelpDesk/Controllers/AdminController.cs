using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;

namespace OnlineHelpDesk.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
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

        //[HttpPost]
        //public async Task<ActionResult> ImportStudents()
        //{
        //    return View();
        //}

        // IMPORT DATA FROM EXCEL
        // MANUAL GENERATE REPORT

        [HttpPost]
        public ActionResult UploadExcel(ImportDataiewModel model)
        {
            if (ModelState.IsValid)
            {
                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //    model.File.InputStream.CopyTo(memoryStream);
                //}

                try
                {
                    var file = model.File;
                    if (file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/Uploaded"), filename);

                        file.SaveAs(path);
                    }

                    //ViewBag.Message = "File upload successfully!";
                    return View("Index");
                }
                catch (Exception e)
                {

                    ViewBag.Message = e.Message;
                    return View("Index");
                }
            }

            return View("Index");
        }
    }
}