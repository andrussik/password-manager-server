using Domain.Entities;

namespace Core.Dtos;

public class UserGroupDto
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string Name { get; set; } = default!;
    public string Role { get; set; } = default!;

    public IList<SecretDto>? Secrets { get; set; }

    public UserGroupDto() { }

    public UserGroupDto(GroupUser groupUser)
    {
        Id = groupUser.Id;
        GroupId = groupUser.GroupId;
        Name = groupUser.Group!.Name;
        Role = GroupRole.GetName(groupUser.GroupRoleId);
        Secrets = groupUser.Group.Secrets?.Select(secret => new SecretDto(secret)).ToList();
    }
}