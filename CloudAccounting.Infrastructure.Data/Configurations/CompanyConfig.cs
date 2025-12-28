using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    internal class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> entity)
        {
            entity.ToTable("GL_COMPANY");
            entity.HasKey(e => e.CompanyCode).HasName("GL_COMPANY_PK");

            entity.Property(e => e.CompanyCode)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("COADDRESS");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("COCITY");
            entity.Property(e => e.Currency)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("COCURRENCY");
            entity.Property(e => e.Fax)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("COFAX");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONAME");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("COPHONE");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("COZIP");
        }
    }
}