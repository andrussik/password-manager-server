using Domain.Entities;

namespace Core.Data.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<string?> GetUserKey(string email);
    Task<bool> Exists(string email, string masterPassword);
}
