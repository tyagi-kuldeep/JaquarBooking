using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IAuditableEntity
    {
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        int CreatedBy { get; set; }
        DateTime CreatedOnUtc { get; set; }
        int? UpdatedBy { get; set; }
        DateTime? UpdatedOnUtc { get; set; }
    }
}
