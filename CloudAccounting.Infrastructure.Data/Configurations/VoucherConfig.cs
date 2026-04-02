using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class VoucherConfig : IEntityTypeConfiguration<VoucherDM>
    {
        public void Configure(EntityTypeBuilder<VoucherDM> entity)
        {
            entity.HasKey(e => e.VoucherCode).HasName("GL_VOUCHER_PK");
            entity.HasIndex(e => e.VoucherType).IsUnique();

            entity.ToTable("GL_VOUCHER");

            entity.Property(e => e.VoucherCode)
                .ValueGeneratedOnAdd()
                .HasColumnType("INT")
                .HasColumnName("VCHCODE");
            entity.Property(e => e.VoucherClassification)
                .HasColumnType("TINYINT")
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