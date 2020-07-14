using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OnlineHelpDesk.Models
{
    public class ImportDataViewModel
    {
        [Required(ErrorMessage ="Please upload a CSV file")]
        public HttpPostedFileBase File { get; set; }
    }
}