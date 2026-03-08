using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class ReconcileReportConfig : IEntityTypeConfiguration<ReconcileReportDM>
    {
        public void Configure(EntityTypeBuilder<ReconcileReportDM> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_RECONCILE_REPORT");

            entity.Property(e => e.Amount)
                .HasColumnType("MONEY")
                .HasColumnName("AMOUNT");
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
            entity.Property(e => e.MonthYear)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("MONTHYEAR");
            entity.Property(e => e.ReportDate)
                .HasColumnType("DATETIME2")
                .HasColumnName("REPORTDATE");
            entity.Property(e => e.Srno)
                .HasColumnType("INT")
                .HasColumnName("SRNO");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
            entity.Property(e => e.VoucherDate)
                .HasColumnType("DATETIME2")
                .HasColumnName("VCHDATE");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("VCHDESCRIPTION");
            entity.Property(e => e.VoucherNumber)
                .HasColumnType("INT")
                .HasColumnName("VCHNO");
            entity.Property(e => e.Reference)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("VCHREFERENCE");
            entity.Property(e => e.VoucherType)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("VCHTYPE");
        }
    }
}