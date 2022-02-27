using Core.Data.Repositories;
using Domain.Entities;

namespace Infrastructure.Db.Repositories;

public class SecretRepository : BaseRepository<Secret>, ISecretRepository
{
    public SecretRepository(AppDbContext dbContext) : base(dbContext) { }
}