namespace CloudAccounting.Infrastructure.Data.Models;

public partial class GroupsDetailDM
{
    public Int16? GroupId { get; set; }

    public Int16? SegmentId { get; set; }

    public Int16? SegmentParent { get; set; }

    public string? SegmentType { get; set; }

    public Int16? PageId { get; set; }

    public string? ItemRole { get; set; }

    public string? AllowAccess { get; set; }

    public virtual GroupsMasterDM? GroupsMasterNavigation { get; set; }

    public virtual SegmentDM? SegmentNavigation { get; set; }
}
