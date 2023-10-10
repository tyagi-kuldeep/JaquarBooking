using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class JB_MasterMenu
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
