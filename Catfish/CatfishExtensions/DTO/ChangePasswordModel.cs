using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CatfishExtensions.DTO
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; } = "";

        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; } = "";

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; } = "";

        [Display(Name = "Confirm new Password")]
        [Required(ErrorMessage = "Confirm new password is required")]
        public string ConfirmPassword { get; set; } = "";
    }
}
