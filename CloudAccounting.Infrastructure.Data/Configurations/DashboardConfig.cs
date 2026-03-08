using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.DataContext.Configurations
{
    public class DashboardConfig : IEntityTypeConfiguration<DashboardDM>
    {
        public void Configure(EntityTypeBuilder<DashboardDM> entity)
        {
            entity
                .HasNoKey()
                .ToTable("GL_DASHBOARD");

            entity.Property(e => e.AccountTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ACCOUNTTITLE");
            entity.Property(e => (decimal?)e.Current_Year)
                .HasColumnType("MONEY")
                .HasColumnName("CURRENT_YEAR");
            entity.Property(e => (decimal?)e.CurrentYear)
                .HasColumnType("MONEY")
                .HasColumnName("CURRENTYEAR");
            entity.Property(e => (decimal?)e.Previous_Year)
                .HasColumnType("MONEY")
                .HasColumnName("PREVIOUS_YEAR");
            entity.Property(e => (decimal?)e.Previousyear)
                .HasColumnType("MONEY")
                .HasColumnName("PREVIOUSYEAR");
            entity.Property(e => e.RatioTitle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RATIOTITLE");
            entity.Property(e => e.Srno)
                .HasColumnType("INT")
                .HasColumnName("SRNO");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERID");
        }
    }
}