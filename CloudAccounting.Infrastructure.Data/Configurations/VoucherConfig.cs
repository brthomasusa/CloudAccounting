using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

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
        
            entity.HasData(
                new VoucherDM
                {
                    VoucherCode = 1,
                    VoucherTitle = "Bank Payment Voucher",
                    VoucherType = "BPV",
                    VoucherClassification = 1
                },
                new VoucherDM
                {
                    VoucherCode = 2,
                    VoucherTitle = "Local Sales Invoice",
                    VoucherType = "LSI",
                    VoucherClassification = 3
                },
                new VoucherDM
                {
                    VoucherCode = 3,
                    VoucherTitle = "Bank Receipt Voucher",
                    VoucherType = "BRV",
                    VoucherClassification = 2
                },
                new VoucherDM
                {
                    VoucherCode = 4,
                    VoucherTitle = "Adjustment Voucher",
                    VoucherType = "ADJ",
                    VoucherClassification = 3
                },
                new VoucherDM
                {
                    VoucherCode = 5,
                    VoucherTitle = "Purchase Order",
                    VoucherType = "PO",
                    VoucherClassification = 1
                }
            );
        }
    }
}