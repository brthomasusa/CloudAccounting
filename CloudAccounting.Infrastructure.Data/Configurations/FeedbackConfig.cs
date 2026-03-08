using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.Infrastructure.Data.Models;

namespace CloudAccounting.Infrastructure.Data.Configurations
{
    public class FeedbackConfig : IEntityTypeConfiguration<FeedbackDM>
    {
        public void Configure(EntityTypeBuilder<FeedbackDM> entity)
        {
            entity.HasKey(e => e.FeedbackId).HasName("GL_FEEDBACK_PK");

            entity.ToTable("GL_FEEDBACK");

            entity.Property(e => e.FeedbackId)
                .ValueGeneratedOnAdd()
                .HasColumnType("INT")
                .HasColumnName("FEEDBACKID");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CUSTEMAIL");
            entity.Property(e => e.CustomerFeedback)
                .IsUnicode(false)
                .HasColumnName("CUSTFEEDBACK");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUSTNAME");
            entity.Property(e => e.Timestamp)
                .HasColumnType("DATETIME2")
                .HasDefaultValueSql("GETDATE()")
                .HasColumnName("TS");
        }
    }
}