using System.Linq.Expressions;
using Domain.Entities;

namespace Core.Data.Repositories;

public interface IGroupUserRepository : IBaseRepository<GroupUser>
{
    Task<IList<GroupUser>> SearchAndIncludeGroupSecrets(Expression<Func<GroupUser, bool>> expression);
}