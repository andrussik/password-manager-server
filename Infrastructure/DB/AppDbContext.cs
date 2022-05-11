using System.Security.Claims;
using Core.Interfaces;
using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public class AppDbContext : DbContext, IUnitOfWork
{
    private const string COLLATION = "db_collation";

    private readonly IHttpContextAccessor _httpContextAccessor = default!;

    public DbSet<Group> Groups { get; set; } = default!;
    public DbSet<GroupRole> GroupRoles { get; set; } = default!;
    public DbSet<GroupUser> GroupUsers { get; set; } = default!;
    public DbSet<Secret> Secrets { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<GroupInvitation> GroupInvitations { get; set; } = default!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    public DbSet<Setting> Settings { get; set; } = default!;

    public DbSet<Culture> Cultures { get; set; } = default!;
    public DbSet<Resource> Resources { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) :
        base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasCollation(COLLATION, "en-u-ks-primary", "icu", false);

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property("Id")
                .HasDefaultValueSql("gen_random_uuid()");
        }

        modelBuilder.Entity<GroupRole>()
            .HasData(GroupRole.Admin, GroupRole.Owner, GroupRole.Writer, GroupRole.Reader);

        modelBuilder.Entity<Culture>()
            .HasData(Culture.English, Culture.Estonian);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<string>().UseCollation(COLLATION);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        UpdateTrackedEntities();

        return await base.SaveChangesAsync(ct);
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default) => await Database.BeginTransactionAsync(ct);
    public async Task CommitAsync(CancellationToken ct = default) => await Database.CommitTransactionAsync(ct);

    private void UpdateTrackedEntities()
    {
        var updateDate = DateTime.Now;
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        foreach (var e in ChangeTracker.Entries<EntityWithMeta>())
        {
            if (e.State == EntityState.Added)
            {
                e.Entity.CreatedAt = updateDate;
                e.Entity.CreatedBy = userId is not null ? Guid.Parse(userId) : null;
            }

            e.Entity.UpdatedAt = updateDate;
            e.Entity.UpdatedBy = userId is not null ? Guid.Parse(userId) : null;
        }
    }
}