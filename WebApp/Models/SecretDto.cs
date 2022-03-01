using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace WebApp.Models;

public class SecretDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Description { get; set; }

    public Guid? UserId { get; set; }

    public Guid? GroupId { get; set; }

    public SecretDto() { }

    public SecretDto(Secret secret)
    {
        Id = secret.Id;
        Name = secret.Name;
        Username = secret.Username;
        Password = secret.Password;
        Description = secret.Description;
        UserId = secret.UserId;
        GroupId = secret.GroupId;
    }
}