using System.Linq.Expressions;
using Core.Data.Repositories;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Db.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : Entity
{
    protected readonly AppDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected BaseRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    public async Task<TEntity> Get(Guid id) => (await DbSet.FindAsync(id))!;

    public async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> expression) =>
        await DbSet.FirstOrDefaultAsync(expression);

    public async Task<IList<TEntity>> Search(Expression<Func<TEntity, bool>> expression) =>
        await DbSet.Where(expression).ToListAsync();

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);

    public void Remove(TEntity entity) => DbSet.Remove(entity);

    public void RemoveRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);
}