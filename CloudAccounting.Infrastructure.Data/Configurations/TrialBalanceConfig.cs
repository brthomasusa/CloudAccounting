using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class TrialBalanceConfig : IEntityTypeConfiguration<TrialBalanceDM>
    {
        public void Configure(EntityTypeBuilder<TrialBalanceDM> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_TRIAL_BALANCE");

            entity.Property(e => e.ActivityCredit)
                .HasColumnType("MONEY")
                .HasColumnName("ACTIVITYCR");
            entity.Property(e => e.ActivityDebit)
                .HasColumnType("MONEY")
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
                .HasColumnType("MONEY")
                .HasColumnName("CLOSINGCR");
            entity.Property(e => e.ClosingDebit)
                .HasColumnType("MONEY")
                .HasColumnName("CLOSINGDR");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COACODE");
            entity.Property(e => e.AccountLevel)
                .HasColumnType("TINYINT")
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
                .HasColumnType("BIT")
                .HasColumnName("GRAND_TOTAL");
            entity.Property(e => e.OpeningCredit)
                .HasColumnType("MONEY")
                .HasColumnName("OPENCR");
            entity.Property(e => e.OpeningDebit)
                .HasColumnType("MONEY")
                .HasColumnName("OPENDR");
            entity.Property(e => e.ReportLevel)
                .HasColumnType("TINYINT")
                .HasColumnName("REPORTLEVEL");
            entity.Property(e => e.Tbdate)
                .HasColumnType("DATETIME2")
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