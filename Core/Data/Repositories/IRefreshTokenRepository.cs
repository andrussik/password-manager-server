using Domain.Entities;

namespace Core.Data.Repositories;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    Task<bool> Exists(string token);
}