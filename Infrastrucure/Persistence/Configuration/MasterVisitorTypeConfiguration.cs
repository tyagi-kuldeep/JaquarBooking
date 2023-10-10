using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastrucure.Persistence.Configuration
{
    public class MasterVisitorTypeConfiguration : IEntityTypeConfiguration<JB_MasterVisitorType>
    {
        public void Configure(EntityTypeBuilder<JB_MasterVisitorType> builder)
        {
            builder.ToTable(nameof(JB_MasterVisitorType));
            builder.Property(nameof(JB_MasterVisitorType.Name)).HasMaxLength(50).IsRequired();
            builder.Property(nameof(JB_MasterVisitorType.Type)).HasMaxLength(8).IsRequired();
        }


    }

}
