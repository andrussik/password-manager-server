using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;
using Domain.Exceptions;

namespace Domain.Entities;

public class Secret : Entity
{
    [MaxLength(1024)]
    public string Name { get; set; } = default!;
    
    [MaxLength(1024)]
    public string? Username { get; set; }
    public string? Password { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }

    public void Validate()
    {
        if (UserId is not null && GroupId is not null)
            throw new SecretException("Invalid secret. Secret cannot have both UserId and GroupId.");
    }

    public void ValidateOwnership(Guid userId)
    {
        if (UserId != userId)
            throw new SecretException("User is not owner of this secret.");
    }
}