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
    public class ParticipantConfiguration : IEntityTypeConfiguration<JB_Participant>
    {
        public void Configure(EntityTypeBuilder<JB_Participant> builder)
        {
            builder.ToTable(nameof(JB_Participant));
            builder.Property(nameof(JB_Participant.Name)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_Participant.Phone)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_Participant.JaquarProductPromoting)).HasMaxLength(500).IsRequired();
            
        }
    }
}
