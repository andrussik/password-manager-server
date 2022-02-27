using Core.Data.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<string?> GetUserKey(string email) =>
        await DbSet.Where(x => x.Email == email).Select(x => x.Key).FirstOrDefaultAsync();

    public Task<bool> Exists(string email, string masterPassword) =>
        DbSet.AnyAsync(x => x.Email == email && x.MasterPasswordHash == masterPassword);
}