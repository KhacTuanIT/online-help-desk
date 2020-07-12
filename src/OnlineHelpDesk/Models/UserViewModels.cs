using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace OnlineHelpDesk.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "User Identity")]
        public string UserIdentity { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Profile picture")]
        public string ProfilePicture { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}