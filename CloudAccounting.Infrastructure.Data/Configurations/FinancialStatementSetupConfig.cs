using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class FinancialStatementSetupConfig : IEntityTypeConfiguration<FinancialStatementSetup>
    {
        public void Configure(EntityTypeBuilder<FinancialStatementSetup> entity)
        {
            entity.HasKey(e => new { e.CompanyCode, e.ReportCode, e.FinancialStatementAccount }).HasName("GL_FS_SETUP_PK");

            entity.ToTable("GL_FS_SETUP");

            entity.Property(e => e.CompanyCode)
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.ReportCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("REPORTCODE");
            entity.Property(e => e.FinancialStatementAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FSACCOUNT");
            entity.Property(e => e.AccountFrom)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("ACCOUNTFROM");
            entity.Property(e => e.AccountTo)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("ACCOUNTTO");
            entity.Property(e => e.ReportTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("REPORTTITLE");
        }
    }
}