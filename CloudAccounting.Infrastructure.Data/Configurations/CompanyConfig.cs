using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    internal class CompanyConfig : IEntityTypeConfiguration<CompanyDM>
    {
        public void Configure(EntityTypeBuilder<CompanyDM> entity)
        {
            entity.ToTable("GL_COMPANY");
            entity.HasKey(e => e.CompanyCode).HasName("GL_COMPANY_PK");
            entity.HasIndex(e => e.CompanyName).IsUnique();                

            entity.Property(e => e.CompanyCode)
                .ValueGeneratedOnAdd()
                .HasColumnType("INT")
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
        
            entity.HasData(
                new CompanyDM
                {
                    CompanyCode = 1,
                    CompanyName = "BTechnical Consulting",
                    Address = "123 Main Street",
                    City = "Dallas",
                    Zipcode = "75201",
                    Phone = "214-555-1234",
                    Fax = "214-555-5678",
                    Currency = "USD"
                },
                new CompanyDM
                {
                    CompanyCode = 2,
                    CompanyName = "Contoso Ltd.",
                    Address = "456 Elm Street",
                    City = "Seattle",
                    Zipcode = "98101",
                    Phone = "206-555-9876",
                    Fax = "206-555-5432",
                    Currency = "USD"
                },
                new CompanyDM
                {
                    CompanyCode = 3,
                    CompanyName = "Fabrikam Inc.",
                    Address = "789 Oak Avenue",
                    City = "Chicago",
                    Zipcode = "60601",
                    Phone = "312-555-2468",
                    Fax = "312-555-1357",
                    Currency = "USD"
                },
                new CompanyDM
                {
                    CompanyCode = 4,
                    CompanyName = "Adventure Works",
                    Address = "321 Pine Road",
                    City = "Denver",
                    Zipcode = "80201",
                    Phone = "303-555-7890",
                    Fax = "303-555-4321",
                    Currency = "USD"
                },
                new CompanyDM
                {
                    CompanyCode = 5,
                    CompanyName = "Contoso Pharmaceuticals",
                    Address = "654 Maple Lane",
                    City = "Boston",
                    Zipcode = "02101",
                    Phone = "617-555-2468",
                    Fax = "617-555-1357",
                    Currency = "USD"
                }
            );
        }
    }
}