using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class BudgetReportConfig : IEntityTypeConfiguration<BudgetReportDM>
    {
        public void Configure(EntityTypeBuilder<BudgetReportDM> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_BUDGET_REPORT");

            entity.Property(e => e.AccountFrom)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("ACCOUNTFROM");
            entity.Property(e => e.AccountTo)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("ACCOUNTTO");
            entity.Property(e => e.Actual)
                .HasColumnType("MONEY")
                .HasColumnName("ACTUAL");
            entity.Property(e => e.Budget)
                .HasColumnType("MONEY")
                .HasColumnName("BUDGET");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COACODE");
            entity.Property(e => e.AccountTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COATITLE");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONAME");
            entity.Property(e => e.GrandTotal)
                .HasColumnType("BIT")
                .HasColumnName("GRAND_TOTAL");
            entity.Property(e => e.MonthFrom)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MONTHFROM");
            entity.Property(e => e.MonthTo)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MONTHTO");
            entity.Property(e => e.Percent)
                .HasColumnType("DECIMAL(7,2)")
                .HasColumnName("PERCENT");
            entity.Property(e => e.PrintedOn)
                .HasColumnType("DATETIME2")
                .HasColumnName("PRINTEDON");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
            entity.Property(e => e.Variance)
                .HasColumnType("MONEY")
                .HasColumnName("VARIANCE");
        }
    }
}