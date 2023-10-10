using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models.Role
{
    public class AddRoleVm
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        public string EncryptedId { get; set; }
    }
}
