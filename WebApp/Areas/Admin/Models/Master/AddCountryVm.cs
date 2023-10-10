using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models.Master
{
    public class AddCountryVm
    {
        public int Id { get; set; }
        //public string EncId { get; set; }
        //public string CallerBtnName { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(100)]
        public string Code { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }

    }
}
