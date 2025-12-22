using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class ReconcileReportConfig : IEntityTypeConfiguration<ReconcileReport>
    {
        public void Configure(EntityTypeBuilder<ReconcileReport> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_RECONCILE_REPORT");

            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(15,2)")
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
                .HasColumnType("DATE")
                .HasColumnName("REPORTDATE");
            entity.Property(e => e.Srno)
                .HasColumnType("NUMBER")
                .HasColumnName("SRNO");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
            entity.Property(e => e.VoucherDate)
                .HasColumnType("DATE")
                .HasColumnName("VCHDATE");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("VCHDESCRIPTION");
            entity.Property(e => e.VoucherNumber)
                .HasPrecision(10)
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