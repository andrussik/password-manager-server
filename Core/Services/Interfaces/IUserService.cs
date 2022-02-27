using Domain.Entities;

namespace Core.Services.Interfaces;

public interface IUserService
{
    Task<User> Get(Guid id);
    Task<User?> Get(string email, string masterPassword);
    Task CreateNewUser(string email, string name, string masterPassword);
}