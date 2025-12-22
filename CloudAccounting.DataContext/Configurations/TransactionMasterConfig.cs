using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class TransactionMasterConfig : IEntityTypeConfiguration<TransactionMaster>
    {
        public void Configure(EntityTypeBuilder<TransactionMaster> entity)
        {
            entity.HasKey(e => e.TransactionNumber).HasName("PK_TRAN_MASTER");

            entity.ToTable("GL_TRAN_MASTER");

            entity.Property(e => e.TransactionNumber)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TRAN_NO");
            entity.Property(e => e.Closing)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("CLOSING");
            entity.Property(e => e.CompanyCode)
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.CompanyMonth)
                .HasPrecision(2)
                .HasColumnName("COMONTHID");
            entity.Property(e => e.CompanyYear)
                .HasPrecision(4)
                .HasColumnName("COYEAR");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CREATEDBY");
            entity.Property(e => e.CreatedOn)
                .HasColumnType("DATE")
                .HasColumnName("CREATEDON");
            entity.Property(e => e.VoucherCode)
                .HasColumnType("NUMBER")
                .HasColumnName("VCHCODE");
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