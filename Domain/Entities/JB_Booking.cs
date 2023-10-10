using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class JB_Booking : BaseEntity
    {
        public int Id { get; set; }

        public int VisitorTypeId { get; set; }
        public string Objective { get; set; }

        public string Location { get; set; }

        public DateTime VisitDateFrom { get; set; }

        public DateTime VisitDateTo { get; set; }

        public int AccomodationId { get; set; }

        public int BranchId { get; set; }

        public string City { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }
        public string Document { get; set; } = string.Empty;
    }
}
