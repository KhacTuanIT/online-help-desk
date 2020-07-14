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
        [Key]
        [Required]
        [Display(Name = "User Identity")]
        public string UserIdentity { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Profile picture")]
        [DataType(DataType.ImageUrl)]
        public string ProfilePicture { get; set; }

        [Display(Name = "Contact")]
        public string Contact { get; set; }
    }
}