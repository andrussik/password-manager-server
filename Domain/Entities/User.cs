using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User : SecretOwner
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string MasterPasswordHash { get; set; } = default!;

    public IList<GroupUser>? GroupUsers { get; set; }
}