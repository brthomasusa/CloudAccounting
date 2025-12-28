using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Core.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.UserId).HasName("GL_USERS_PK");

            entity.ToTable("GL_USERS");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
            entity.Property(e => e.Admin)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ADMIN");
            entity.Property(e => e.CompanyCode)
                .HasColumnType("NUMBER")
                .HasColumnName("COCODE");
            entity.Property(e => e.CompanyMonthId)
                .HasPrecision(2)
                .HasColumnName("COMONTHID");
            entity.Property(e => e.CompanyYear)
                .HasPrecision(4)
                .HasColumnName("COYEAR");
            entity.Property(e => e.GroupId)
                .HasPrecision(4)
                .HasColumnName("GROUPID");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");

            entity.HasOne(d => d.CompanyCodeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyCode)
                .HasConstraintName("FK_USERS");

            entity.HasOne(d => d.GroupsMasterNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_USERS2");
        }
    }
}