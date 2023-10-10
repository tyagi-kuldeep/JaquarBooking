using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastrucure.Persistence.Configuration
{
    public class MasterMenuConfiguration : IEntityTypeConfiguration<JB_MasterMenu>
    {
        public void Configure(EntityTypeBuilder<JB_MasterMenu> builder)
        {
            builder.ToTable(nameof(JB_MasterMenu));
            builder.Property(nameof(JB_MasterMenu.ParentMenu)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_MasterMenu.SubMenu)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_MasterMenu.Controller)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_MasterMenu.Action)).HasMaxLength(100).IsRequired();
            builder.Property(nameof(JB_MasterMenu.Order)).IsRequired();
            builder.Property(nameof(JB_MasterMenu.SubOrder)).IsRequired();

        }
    }
}

