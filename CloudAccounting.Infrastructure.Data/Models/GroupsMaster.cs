namespace CloudAccounting.Infrastructure.Data.Models;

public partial class GroupsMasterDM
{
    public Int16 GroupId { get; set; }

    public string? GroupTitle { get; set; }

    public virtual ICollection<UserDM> Users { get; set; } = [];
}
