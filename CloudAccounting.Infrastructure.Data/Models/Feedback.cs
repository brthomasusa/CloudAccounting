namespace CloudAccounting.Infrastructure.Data.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerEmail { get; set; }

    public string? CustomerFeedback { get; set; }
}
