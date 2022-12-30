using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastrucure.Persistence.Configuration
{
   public class MasterStateConfiguration : IEntityTypeConfiguration<JB_MasterState>
    {
        public void Configure(EntityTypeBuilder<JB_MasterState> builder)
        {
            builder.ToTable(nameof(JB_MasterState));
            builder.Property(nameof(JB_MasterState.Name)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_MasterState.Code)).HasMaxLength(3).IsRequired();

        }
    }
}

