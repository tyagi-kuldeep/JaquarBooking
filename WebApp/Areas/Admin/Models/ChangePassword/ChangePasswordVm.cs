using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models.ChangePassword
{
    public class ChangePasswordVm
    {
        [Required, Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required, Display(Name = "New Password")]
        public string NewPassword { get; set; } 
        
        [Required, Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}
