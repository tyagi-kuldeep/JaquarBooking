using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.Models.Booking
{
    public class AddBookingVm
    {
        public AddBookingVm()
        {
            Participant = new AddParticipantVm();
        }
        public int Id { get; set; }

        public int? VisitorType { get; set; }
        public int? VisitorTypeId { get; set; }
        public string Objective { get; set; }

        public string Location { get; set; }

        public DateTime VisitDateFrom { get; set; }

        public DateTime VisitDateTo { get; set; }

        public int? AccomodationId { get; set; }
        public string City { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }
        public int? BranchId { get; set; }

        public int? BookingStatusId { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public IFormFile Doc { get; set; }
        public string Document { get; set; } = string.Empty;

        public AddParticipantVm Participant { get; set; }
    }

    public class AddParticipantVm
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int VisitorTypeId { get; set; }
        public int IsHospitalityProjectId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string JaquarProductPromoting { get; set; }
        public string Document { get; set; } = string.Empty;

        public DateTime FlightStartDate { get; set; }

        public DateTime FlightReturnDate { get; set; }
    }
}
