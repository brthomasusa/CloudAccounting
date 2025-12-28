using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class TrialBalanceConfig : IEntityTypeConfiguration<TrialBalance>
    {
        public void Configure(EntityTypeBuilder<TrialBalance> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_TRIAL_BALANCE");

            entity.Property(e => e.ActivityCredit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("ACTIVITYCR");
            entity.Property(e => e.ActivityDebit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("ACTIVITYDR");
            entity.Property(e => e.CostCenterCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("CCCODE");
            entity.Property(e => e.CostCenterTitle)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("CCTITLE");
            entity.Property(e => e.ClosingCredit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("CLOSINGCR");
            entity.Property(e => e.ClosingDebit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("CLOSINGDR");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COACODE");
            entity.Property(e => e.AccountLevel)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("COALEVEL");
            entity.Property(e => e.AccountTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COATITLE");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONAME");
            entity.Property(e => e.FromAccount)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("FROMACCOUNT");
            entity.Property(e => e.GrandTotal)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("GRAND_TOTAL");
            entity.Property(e => e.OpeningCredit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("OPENCR");
            entity.Property(e => e.OpeningDebit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("OPENDR");
            entity.Property(e => e.ReportLevel)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("REPORTLEVEL");
            entity.Property(e => e.Tbdate)
                .HasColumnType("DATE")
                .HasColumnName("TBDATE");
            entity.Property(e => e.ToAccount)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("TOACCOUNT");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
        }
    }
}