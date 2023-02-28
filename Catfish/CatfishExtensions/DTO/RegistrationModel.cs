using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Confirm password is required")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = "";

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = "";

        [Display(Name = "System Roles")]
        public List<string> SystemRoles { get; protected set; } = new List<string>();

    }
}
