using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class GroupsMasterConfig : IEntityTypeConfiguration<GroupsMaster>
    {
        public void Configure(EntityTypeBuilder<GroupsMaster> entity)
        {
            entity.HasKey(e => e.GroupId).HasName("GL_GROUPS_PK");

            entity.ToTable("GL_GROUPS_MASTER");

            entity.Property(e => e.GroupId)
                .HasPrecision(4)
                .HasColumnName("GROUPID");
            entity.Property(e => e.GroupTitle)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("GROUPTITLE");
        }
    }
}