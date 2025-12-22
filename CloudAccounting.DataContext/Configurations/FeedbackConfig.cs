using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.DataContext.Configurations
{
    public class FeedbackConfig : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> entity)
        {
            entity.HasKey(e => e.FeedbackId).HasName("GL_FEEDBACK_PK");

            entity.ToTable("GL_FEEDBACK");

            entity.Property(e => e.FeedbackId)
                .HasColumnType("NUMBER")
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
                .HasPrecision(6)
                .HasDefaultValueSql("sysdate")
                .HasColumnName("TS");
        }
    }
}