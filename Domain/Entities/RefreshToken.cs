using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Token), IsUnique = true)]
public class RefreshToken : Entity
{
    public string Token { get; set; } = default!;
    public string JwtId { get; set; } = default!;
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}