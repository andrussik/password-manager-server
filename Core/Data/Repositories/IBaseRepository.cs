using System.Linq.Expressions;
using Domain.Entities.Base;

namespace Core.Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> Get(Guid id);
    Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> expression);
    Task<IList<TEntity>> Search(Expression<Func<TEntity, bool>> expression);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}