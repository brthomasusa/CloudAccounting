using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class GroupsDetailConfig : IEntityTypeConfiguration<GroupsDetail>
    {
        public void Configure(EntityTypeBuilder<GroupsDetail> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_GROUPS_DETAIL");

            entity.Property(e => e.AllowAccess)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ALLOW_ACCESS");
            entity.Property(e => e.GroupId)
                .HasPrecision(4)
                .HasColumnName("GROUPID");
            entity.Property(e => e.ItemRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ITEMROLE");
            entity.Property(e => e.PageId)
                .HasPrecision(4)
                .HasColumnName("PAGEID");
            entity.Property(e => e.SegmentId)
                .HasColumnType("NUMBER")
                .HasColumnName("SEGMENTID");
            entity.Property(e => e.SegmentParent)
                .HasColumnType("NUMBER")
                .HasColumnName("SEGMENTPARENT");
            entity.Property(e => e.SegmentType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("SEGMENTTYPE");

            entity.HasOne(d => d.GroupsMasterNavigation).WithMany()
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_GROUP_DETAIL");

            entity.HasOne(d => d.SegmentNavigation).WithMany()
                .HasForeignKey(d => d.SegmentId)
                .HasConstraintName("FK_USER_GROUPS");
        }
    }
}