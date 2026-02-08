using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
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
                .HasColumnType("NUMBER(1,0)")
                .HasColumnName("VCHNATURE")
                .HasConversion<int>();
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