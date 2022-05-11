using Domain.Entities;

namespace Core.Interfaces;

public interface IUserService
{
    bool IsAuthenticated { get; }
    Guid CurrentUserId { get; }
    Task<User> GetCurrentUser();
    Task<User> Get(Guid id);
    Task<User?> Get(string email, string masterPassword);
    Task CreateNewUser(string email, string name, string masterPassword);
}