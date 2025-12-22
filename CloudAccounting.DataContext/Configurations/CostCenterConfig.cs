using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class CostCenterConfig : IEntityTypeConfiguration<CostCenter>
    {
        public void Configure(EntityTypeBuilder<CostCenter> entity)
        {
            entity.HasKey(e => new { e.CompanyCode, e.CostCenterCode }).HasName("GL_COST_CENTER_PK");

            entity.ToTable("GL_COST_CENTER");

            entity.Property(e => e.CompanyCode)
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.CostCenterCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CCCODE");
            entity.Property(e => e.CostCenterLevel)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("CCLEVEL");
            entity.Property(e => e.CostCenterTitle)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CCTITLE");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany(p => p.CostCenters)
                .HasForeignKey(d => d.CompanyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COST_CENTER");
        }
    }
}