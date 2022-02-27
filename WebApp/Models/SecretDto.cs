using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebApp.Models;

public class SecretDto
{
    public Guid Id { get; set; }
    [MaxLength(1024)] public string Name { get; set; } = default!;

    [MaxLength(1024)] public string? Username { get; set; }
    public string? Password { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GroupId { get; set; }

    public SecretDto() { }

    public SecretDto(Secret secret)
    {
        Id = secret.Id;
        Name = secret.Name;
        Username = secret.Username;
        Password = secret.Password;
        UserId = secret.UserId;
        GroupId = secret.GroupId;
    }
}