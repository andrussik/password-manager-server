using Domain.Entities.Base;

namespace Domain.Entities;

public class Group : SecretOwner
{
    public string Name { get; set; } = default!;

    public IList<GroupUser>? GroupUsers { get; set; }
}