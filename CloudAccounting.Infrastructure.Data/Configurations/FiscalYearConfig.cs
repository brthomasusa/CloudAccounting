using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class FiscalYearConfig : IEntityTypeConfiguration<FiscalYearDM>
    {
        public void Configure(EntityTypeBuilder<FiscalYearDM> entity)
        {
            entity.HasKey(e => new { e.CompanyCode, e.CompanyYear, e.CompanyMonthId }).HasName("GL_FISCAL_YEAR_PK");

            entity.ToTable("GL_FISCAL_YEAR");

            entity.Property(e => e.CompanyCode)
                .HasColumnType("INT")
                .HasColumnName("COCODE");
            entity.Property(e => e.CompanyYear)
                .HasColumnType("SMALLINT")
                .HasColumnName("COYEAR");
            entity.Property(e => e.CompanyMonthId)
                .HasColumnType("TINYINT")
                .HasColumnName("COMONTHID");
            entity.Property(e => e.CompanyMonthName)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("COMONTHNAME");
            entity.Property(e => e.InitialYear)
                .HasColumnType("BIT")
                .HasColumnName("INITIAL_YEAR");
            entity.Property(e => e.MonthClosed)
                .HasColumnType("BIT")
                .HasColumnName("MONTH_CLOSED");
            entity.Property(e => e.PeriodFrom)
                .HasColumnType("DATETIME2")
                .HasColumnName("PFROM");
            entity.Property(e => e.PeriodTo)
                .HasColumnType("DATETIME2")
                .HasColumnName("PTO");
            entity.Property(e => e.TyeExecuted)
                .HasColumnType("DATE")
                .HasColumnName("TYE_EXECUTED");
            entity.Property(e => e.YearClosed)
                .HasColumnType("BIT")
                .HasColumnName("YEAR_CLOSED");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany(p => p.FiscalYears)
                .HasForeignKey(d => d.CompanyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FISCAL_YEAR");

        }
    }
}