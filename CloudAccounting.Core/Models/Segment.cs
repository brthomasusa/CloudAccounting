namespace CloudAccounting.Core.Models;

public partial class Segment
{
    public int SegmentId { get; set; }

    public string? SegmentTitle { get; set; }

    public int? SegmentParent { get; set; }

    public string? SegmentType { get; set; }

    public Int16? PageId { get; set; }

    public string? ItemRole { get; set; }
}
