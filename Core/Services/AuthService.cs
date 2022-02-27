using Core.Services.Interfaces;

namespace Core.Services;

public class AuthService : IAuthService
{
    public Task<bool> Authenticate(string email, string password)
    {
        return Task.Run(() => true);
    }
}