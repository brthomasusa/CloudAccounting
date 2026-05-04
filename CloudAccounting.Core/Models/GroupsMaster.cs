namespace CloudAccounting.Core.Models;

public class GroupsMaster
{
    public Int16 GroupId { get; set; }

    public string? GroupTitle { get; set; }

    public virtual ICollection<User> Users { get; set; } = [];
}
