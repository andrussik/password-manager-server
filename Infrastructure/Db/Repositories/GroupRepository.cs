using Core.Data.Repositories;
using Domain.Entities;

namespace Infrastructure.Db.Repositories;

public class GroupRepository : BaseRepository<Group>, IGroupRepository
{
    public GroupRepository(AppDbContext dbContext) : base(dbContext) { }
}