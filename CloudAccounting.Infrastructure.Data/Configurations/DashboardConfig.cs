using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.DataContext.Configurations
{
    public class DashboardConfig : IEntityTypeConfiguration<Dashboard>
    {
        public void Configure(EntityTypeBuilder<Dashboard> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_DASHBOARD");

            entity.Property(e => e.AccountTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ACCOUNTTITLE");
            entity.Property(e => (decimal?)e.Current_Year)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("CURRENT_YEAR");
            entity.Property(e => (decimal?)e.CurrentYear)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("CURRENTYEAR");
            entity.Property(e => (decimal?)e.Previous_Year)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("PREVIOUS_YEAR");
            entity.Property(e => (decimal?)e.Previousyear)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("PREVIOUSYEAR");
            entity.Property(e => e.RatioTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RATIOTITLE");
            entity.Property(e => e.Srno)
                .HasColumnType("NUMBER")
                .HasColumnName("SRNO");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
        }
    }
}