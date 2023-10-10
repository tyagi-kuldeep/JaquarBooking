using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class JB_Participant : BaseEntity
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
