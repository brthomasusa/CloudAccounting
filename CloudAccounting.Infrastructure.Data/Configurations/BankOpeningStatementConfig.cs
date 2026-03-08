using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    internal class BankOpeningStatementConfig : IEntityTypeConfiguration<BankOpeningStatementDM>
    {
        public void Configure(EntityTypeBuilder<BankOpeningStatementDM> entity)
        {
            entity.HasKey(e => e.SrNo).HasName("PK_BANKS_OS");
            entity.ToTable("GL_BANKS_OS");

            entity.Property(e => e.SrNo)
                .ValueGeneratedOnAdd()
                .HasColumnType("INT")
                .HasColumnName("SR_NO");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("COACODE");
            entity.Property(e => e.CompanyCode)
                .HasColumnType("INT")
                .HasColumnName("COCODE");
            entity.Property(e => e.Reconciled)
                .HasColumnType("BIT")
                .HasColumnName("RECONCILED");
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.Credit)
                .HasColumnType("MONEY")
                .HasColumnName("VCHCR");
            entity.Property(e => e.Debit)
                .HasColumnType("MONEY")
                .HasColumnName("VCHDR");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany(p => p.BankOpeningStatements)
                .HasForeignKey(d => d.CompanyCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BANKS_OS1");

            entity.HasOne(d => d.COANavigation).WithMany(p => p.BankOpeningStatements)
                .HasForeignKey(d => new { d.CompanyCode, d.AccountCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BANKS_OS2");
        }
    }
}