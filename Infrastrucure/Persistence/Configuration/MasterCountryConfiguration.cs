using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class MasterCountryConfiguration : IEntityTypeConfiguration<JB_MasterCountry>
    {
        public void Configure(EntityTypeBuilder<JB_MasterCountry> builder)
        {
            builder.ToTable(nameof(JB_MasterCountry));
            builder.Property(nameof(JB_MasterCountry.Name)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_MasterCountry.Code)).HasMaxLength(3).IsRequired();

        }
    }
}
