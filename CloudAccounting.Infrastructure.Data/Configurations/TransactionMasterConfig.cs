using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class TransactionMasterConfig : IEntityTypeConfiguration<TransactionMasterDM>
    {
        public void Configure(EntityTypeBuilder<TransactionMasterDM> entity)
        {
            entity.HasKey(e => e.TransactionNumber).HasName("PK_TRAN_MASTER");

            entity.ToTable("GL_TRAN_MASTER");

            entity.Property(e => e.TransactionNumber)
                .ValueGeneratedOnAdd()
                .HasColumnType("INT")
                .HasColumnName("TRAN_NO");
            entity.Property(e => e.Closing)
                .HasColumnType("BIT")
                .HasColumnName("CLOSING");
            entity.Property(e => e.CompanyCode)
                .HasColumnType("INT")
                .HasColumnName("COCODE");
            entity.Property(e => e.CompanyMonth)
                .HasColumnType("TINYINT")
                .HasColumnName("COMONTHID");
            entity.Property(e => e.CompanyYear)
                .HasColumnType("SMALLINT")
                .HasColumnName("COYEAR");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.CreatedOn)
                .HasColumnType("DATETIME2")
                .HasColumnName("CREATEDON");
            entity.Property(e => e.VoucherCode)
                .HasColumnType("INT")
                .HasColumnName("VCHCODE");
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
            entity.Property(e => e.Posted)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("VCHPOSTED");
            entity.Property(e => e.Verified)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("VCHVERIFIED");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany(p => p.TransactionMasters)
                .HasForeignKey(d => d.CompanyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRAN_MASTER1");

            entity.HasOne(d => d.VoucherNavigation).WithMany(p => p.TransactionMasters)
                .HasForeignKey(d => d.VoucherCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRAN_MASTER2");

            entity.HasOne(d => d.FiscalYearNavigation).WithMany(p => p.TransactionMasters)
                .HasForeignKey(d => new { d.CompanyCode, d.CompanyYear, d.CompanyMonth })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRAN_MASTER3");
        }
    }
}