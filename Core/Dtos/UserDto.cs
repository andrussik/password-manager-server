using Domain.Entities;

namespace Core.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Key { get; set; } = default!;

    public List<SecretDto>? Secrets { get; set; }
    public List<UserGroupDto>? Groups { get; set; }

    public UserDto() { }

    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        Key = user.Key;
        Secrets = user.Secrets?.Select(secret => new SecretDto(secret)).ToList();
        Groups = user.GroupUsers?.Select(group => new UserGroupDto(group)).ToList();
    }
}