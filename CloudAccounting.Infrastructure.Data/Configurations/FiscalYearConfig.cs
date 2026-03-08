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
        
        
            entity.HasData(
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 1,
                    CompanyMonthName = "January",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 1, 1),
                    PeriodTo = new DateTime(2024, 1, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 2,
                    CompanyMonthName = "February",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 2, 1),
                    PeriodTo = new DateTime(2024, 2, 28),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 3,
                    CompanyMonthName = "March",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 3, 1),
                    PeriodTo = new DateTime(2024, 3, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 4,
                    CompanyMonthName = "April",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 4, 1),
                    PeriodTo = new DateTime(2024, 4, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 5,
                    CompanyMonthName = "May",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 5, 1),
                    PeriodTo = new DateTime(2024, 5, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 6,
                    CompanyMonthName = "June",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 6, 1),
                    PeriodTo = new DateTime(2024, 6, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 7,
                    CompanyMonthName = "July",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 7, 1),
                    PeriodTo = new DateTime(2024, 7, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 8,
                    CompanyMonthName = "August",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 8, 1),
                    PeriodTo = new DateTime(2024, 8, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 9,
                    CompanyMonthName = "September",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 9, 1),
                    PeriodTo = new DateTime(2024, 9, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 10,
                    CompanyMonthName = "October",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 10, 1),
                    PeriodTo = new DateTime(2024, 10, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 11,
                    CompanyMonthName = "November",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 11, 1),
                    PeriodTo = new DateTime(2024, 11, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2024,
                    CompanyMonthId = 12,
                    CompanyMonthName = "December",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2024, 12, 1),
                    PeriodTo = new DateTime(2024, 12, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 1,
                    CompanyMonthName = "January",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 1, 1),
                    PeriodTo = new DateTime(2025, 1, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 2,
                    CompanyMonthName = "February",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 2, 1),
                    PeriodTo = new DateTime(2025, 2, 28),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 3,
                    CompanyMonthName = "March",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 3, 1),
                    PeriodTo = new DateTime(2025, 3, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 4,
                    CompanyMonthName = "April",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 4, 1),
                    PeriodTo = new DateTime(2025, 4, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 5,
                    CompanyMonthName = "May",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 5, 1),
                    PeriodTo = new DateTime(2025, 5, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 6,
                    CompanyMonthName = "June",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 6, 1),
                    PeriodTo = new DateTime(2025, 6, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 7,
                    CompanyMonthName = "July",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 7, 1),
                    PeriodTo = new DateTime(2025, 7, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 8,
                    CompanyMonthName = "August",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 8, 1),
                    PeriodTo = new DateTime(2025, 8, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 9,
                    CompanyMonthName = "September",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 9, 1),
                    PeriodTo = new DateTime(2025, 9, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 10,
                    CompanyMonthName = "October",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 10, 1),
                    PeriodTo = new DateTime(2025, 10, 31),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 11,
                    CompanyMonthName = "November",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 11, 1),
                    PeriodTo = new DateTime(2025, 11, 30),
                    TyeExecuted = null,
                    YearClosed = false
                },
                new FiscalYearDM
                {
                    CompanyCode = 1,
                    CompanyYear = 2025,
                    CompanyMonthId = 12,
                    CompanyMonthName = "December",
                    InitialYear = true,
                    MonthClosed = false,
                    PeriodFrom = new DateTime(2025, 12, 1),
                    PeriodTo = new DateTime(2025, 12, 31),
                    TyeExecuted = null,
                    YearClosed = false
                }                
            );
        }
    }
}