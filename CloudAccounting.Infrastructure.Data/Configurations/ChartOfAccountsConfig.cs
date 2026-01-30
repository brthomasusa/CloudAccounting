using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class ChartOfAccountsConfig : IEntityTypeConfiguration<ChartOfAccounts>
    {
        public void Configure(EntityTypeBuilder<ChartOfAccounts> entity)
        {
            entity.HasKey(e => new { e.CompanyCode, e.AccountCode }).HasName("GL_COA_PK");

            entity.ToTable("GL_COA");

            entity.Property(e => e.CompanyCode)
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COACODE");
            entity.Property(e => e.CostCenterCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CCCODE");
            entity.Property(e => e.AccountLevel)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("COALEVEL");
            entity.Property(e => e.AccountClassification)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COANATURE");
            entity.Property(e => e.AccountTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COATITLE");
            entity.Property(e => e.AccountType)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COATYPE");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany(p => p.ChartOfAccounts)
                .HasForeignKey(d => d.CompanyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COA");
        }
    }
}