using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class TransactionDetailConfig : IEntityTypeConfiguration<TransactionDetail>
    {
        public void Configure(EntityTypeBuilder<TransactionDetail> entity)
        {
            entity.HasKey(e => e.LineNumber).HasName("PK_TRAN_DETAIL");

            entity.ToTable("GL_TRAN_DETAIL");

            entity.Property(e => e.LineNumber)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("LINE_NO");
            entity.Property(e => e.CostCenterCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("CCCODE");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("COACODE");
            entity.Property(e => e.CompanyCode)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.Reconciled)
                .HasColumnType("NUMBER(1)")
                .HasColumnName("RECONCILED");
            entity.Property(e => e.TransactionNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("TRAN_NO");
            entity.Property(e => e.Credit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VCHCR");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("VCHDESCRIPTION");
            entity.Property(e => e.Debit)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VCHDR");
            entity.Property(e => e.Reference)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("VCHREFERENCE");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.CompanyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRAN_DETAIL1");

            entity.HasOne(d => d.TransactionMasterNavigation).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => d.TransactionNumber)
                .HasConstraintName("FK_TRAN_DETAIL2");

            entity.HasOne(d => d.CostCenterNavigation).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => new { d.CompanyCode, d.CostCenterCode })
                .HasConstraintName("FK_TRAN_DETAIL3");

            entity.HasOne(d => d.COANavigation).WithMany(p => p.TransactionDetails)
                .HasForeignKey(d => new { d.CompanyCode, d.AccountCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TRAN_DETAIL4");
        }
    }
}