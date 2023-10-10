using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Persistence.Configuration
{
    public class MasterBookigConfiguration : IEntityTypeConfiguration<JB_Booking>
    {
        public void Configure(EntityTypeBuilder<JB_Booking> builder)
        {
            builder.ToTable(nameof(JB_Booking));
            builder.Property(nameof(JB_Booking.City)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_Booking.Description)).HasMaxLength(100).IsRequired();
        }
    }
}
