using System.Linq.Expressions;
using Core.Data.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Repositories;

public class GroupUserRepository : BaseRepository<GroupUser>, IGroupUserRepository
{
    public GroupUserRepository(AppDbContext dbContext) : base(dbContext) { }

    public async Task<IList<GroupUser>> SearchAndIncludeGroupSecrets(Expression<Func<GroupUser, bool>> expression)
        => await DbSet.Include(x => x.Group!.Secrets).Where(expression).ToListAsync();
}