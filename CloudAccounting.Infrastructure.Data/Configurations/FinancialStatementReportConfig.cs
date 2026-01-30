using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class FinancialStatementReportConfig : IEntityTypeConfiguration<FinancialStatementReport>
    {
        public void Configure(EntityTypeBuilder<FinancialStatementReport> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_FS_REPORT");

            entity.Property(e => e.Calculation)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("CALCULATION");
            entity.Property(e => e.CompanyMonthName)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("COMONTHNAME");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONAME");
            entity.Property(e => e.CompanyYear)
                .HasPrecision(4)
                .HasColumnName("COYEAR");
            entity.Property(e => e.CurrentBalance)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("CURRENTBALANCE");
            entity.Property(e => e.FinancialStatementAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FSACCOUNT");
            entity.Property(e => e.Heading)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("HEADING");
            entity.Property(e => e.NetValue)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("NETVALUE");
            entity.Property(e => e.Notes)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("NOTES");
            entity.Property(e => e.NotesCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("NOTESCODE");
            entity.Property(e => e.NotesTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOTESTITLE");
            entity.Property(e => e.Percent)
                .HasColumnType("NUMBER(7,2)")
                .HasColumnName("PERCENT");
            entity.Property(e => e.PreviousBalance)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("PREVIOUSBALANCE");
            entity.Property(e => e.ReportCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("REPORTCODE");
            entity.Property(e => e.ReportTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("REPORTTITLE");
            entity.Property(e => e.Srno)
                .HasColumnType("NUMBER")
                .HasColumnName("SRNO");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
        }
    }
}