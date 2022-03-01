using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db;

public class AppDbContext : DbContext
{
    public DbSet<Group> Groups { get; set; } = default!;
    public DbSet<GroupRole> GroupRoles { get; set; } = default!;
    public DbSet<GroupUser> GroupUsers { get; set; } = default!;
    public DbSet<Secret> Secrets { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<GroupInvitation> GroupInvitations { get; set; } = default!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        modelBuilder.Entity<GroupRole>()
            .HasData(GroupRole.Admin, GroupRole.Owner, GroupRole.Writer, GroupRole.Reader);
    }
}