using Domain.Entities.Base;

namespace Domain.Entities;

public class User : SecretOwner
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string MasterPasswordHash { get; set; } = default!;

    public IList<GroupUser>? GroupUsers { get; set; }
}