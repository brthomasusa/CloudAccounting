using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class BudgetConfig : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_BUDGET");

            entity.Property(e => e.BudgetAmount1)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT1");
            entity.Property(e => e.BudgetAmount10)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT10");
            entity.Property(e => e.BudgetAmount11)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT11");
            entity.Property(e => e.BudgetAmount12)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT12");
            entity.Property(e => e.BudgetAmount2)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT2");
            entity.Property(e => e.BudgetAmount3)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT3");
            entity.Property(e => e.BudgetAmount4)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT4");
            entity.Property(e => e.BudgetAmount5)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT5");
            entity.Property(e => e.BudgetAmount6)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT6");
            entity.Property(e => e.BudgetAmount7)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT7");
            entity.Property(e => e.BudgetAmount8)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT8");
            entity.Property(e => e.BudgetAmount9)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("BUDGET_AMOUNT9");
            entity.Property(e => e.CostCenterCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CCCODE");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COACODE");
            entity.Property(e => e.AccountClassification)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COANATURE");
            entity.Property(e => e.CompanyCode)
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.CompanyYear)
                .HasPrecision(4)
                .HasColumnName("COYEAR");
            entity.Property(e => e.Criterion)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("CRITERION");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany()
                .HasForeignKey(d => d.CompanyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BUDGET1");

            entity.HasOne(d => d.COANavigation).WithMany()
                .HasForeignKey(d => new { d.CompanyCode, d.AccountCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BUDGET2");
        }
    }
}