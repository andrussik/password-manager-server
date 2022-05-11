using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    
    DbSet<Group> Groups{ get; }
    DbSet<GroupRole> GroupRoles{ get; }
    DbSet<GroupUser> GroupUsers{ get; }
    DbSet<Secret> Secrets{ get; }
    DbSet<User> Users{ get; }
    DbSet<GroupInvitation> GroupInvitations{ get; }
    DbSet<RefreshToken> RefreshTokens{ get; }
}