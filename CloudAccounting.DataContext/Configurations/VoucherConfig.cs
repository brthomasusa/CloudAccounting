using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class VoucherConfig : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> entity)
        {
            entity.HasKey(e => e.VoucherCode).HasName("GL_VOUCHER_PK");

            entity.ToTable("GL_VOUCHER");

            entity.Property(e => e.VoucherCode)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("VCHCODE");
            entity.Property(e => e.VoucherNature)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("VCHNATURE");
            entity.Property(e => e.VoucherTitle)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("VCHTITLE");
            entity.Property(e => e.VoucherType)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("VCHTYPE");
        }
    }
}