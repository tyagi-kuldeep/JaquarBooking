namespace WebApp.Areas.Admin.Models.Participant
{
    public class ListParticipantVm
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
        public DateTime CreatedDate { get; set; }

        public DateTime FlightStartDate { get; set; }

        public DateTime FlightReturnDate { get; set; }
        public string EncryptedId { get; set; }
    }
}
