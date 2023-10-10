using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Account
{
    public class LoginVm
    {
        [StringLength(200), EmailAddress]
        public string Email { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
    }
}