using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.Admin.Models.Booking
{
    public class ListBookingVm
    {
        public int Id { get; set; }

        public IList<SelectListItem> VisitorTypeId { get; set; }
        public string Objective { get; set; }

        public string Location { get; set; }

        public DateTime VisitDateFrom { get; set; }

        public DateTime VisitDateTo { get; set; }

        public IList<SelectListItem> Accomodation { get; set; }

        public IList<SelectListItem> Branch { get; set; }

        public string City { get; set; }

        public string Description { get; set; }
                
        public string Phone { get; set; }
        public IList<SelectListItem> BookingStatus{ get; set; }
                
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EncryptedId { get; set; }
        public IFormFile Doc { get; set; }
        public string Document { get; set; } = string.Empty;

    }
  
}
