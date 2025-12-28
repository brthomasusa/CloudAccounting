namespace CloudAccounting.Core.Models;

public partial class GroupsDetail
{
    public Int16? GroupId { get; set; }

    public int? SegmentId { get; set; }

    public int? SegmentParent { get; set; }

    public string? SegmentType { get; set; }

    public Int16? PageId { get; set; }

    public string? ItemRole { get; set; }

    public string? AllowAccess { get; set; }

    public virtual GroupsMaster? GroupsMasterNavigation { get; set; }

    public virtual Segment? SegmentNavigation { get; set; }
}
