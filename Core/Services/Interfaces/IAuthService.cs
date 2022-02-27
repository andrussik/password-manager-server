namespace Core.Services.Interfaces;

public interface IAuthService
{
    public Task<bool> Authenticate(string email, string password);
}