using Domain.Entities.Base;

namespace Domain.Entities;

public class GroupUser : Entity
{
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid GroupRoleId { get; set; }
    public GroupRole? GroupRole { get; set; }

    public bool WriteAllowed() => GroupRoleId == GroupRole.Owner.Id
                                  || GroupRoleId == GroupRole.Admin.Id
                                  || GroupRoleId == GroupRole.Writer.Id;

    public bool UpdateNameAllowed() => WriteAllowed();
}