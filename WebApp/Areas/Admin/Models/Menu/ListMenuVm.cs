using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models.Menu
{
    public class ListMenuVm
    {
        [Key]
        public int Id { get; set; }
        public string ParentMenu { get; set; }
        public string SubMenu { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }

        public int Order { get; set; }
        public int SubOrder { get; set; }
    }
}
