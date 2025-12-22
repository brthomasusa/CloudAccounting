using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class SegmentConfig : IEntityTypeConfiguration<Segment>
    {
        public void Configure(EntityTypeBuilder<Segment> entity)
        {
            entity.HasKey(e => e.SegmentId).HasName("GL_SEGMENTS_PK");

            entity.ToTable("GL_SEGMENTS");

            entity.Property(e => e.SegmentId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SEGMENTID");
            entity.Property(e => e.ItemRole)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ITEMROLE");
            entity.Property(e => e.PageId)
                .HasPrecision(4)
                .HasColumnName("PAGEID");
            entity.Property(e => e.SegmentParent)
                .HasColumnType("NUMBER")
                .HasColumnName("SEGMENTPARENT");
            entity.Property(e => e.SegmentTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SEGMENTTITLE");
            entity.Property(e => e.SegmentType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("SEGMENTTYPE");
        }
    }
}