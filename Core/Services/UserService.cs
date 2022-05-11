using System.Security.Claims;
using Core.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _uow;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor)
    {
        _uow = uow;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid CurrentUserId =>
        Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

    public Task<User> GetCurrentUser() => Get(CurrentUserId);

    public async Task<User> Get(Guid id)
    {
        var user = await _uow.Users.FindAsync(id);

        var userSecrets = await _uow.Secrets.Where(x => x.UserId == id).ToListAsync();
        user!.Secrets = userSecrets.ToList();

        var userGroups = await _uow.GroupUsers
            .Include(x => x.Group!.Secrets)
            .Where(x => x.UserId == id)
            .ToListAsync();

        user.GroupUsers = userGroups;

        return user;
    }

    public async Task<User?> Get(string email, string masterPassword)
    {
        var userKey = await _uow.Users.Where(x => x.Email == email).Select(x => x.Key).FirstOrDefaultAsync();
        if (userKey is null)
            return null;

        var salt = Convert.FromBase64String(userKey);

        var masterPasswordHash = Crypto.Crypto.GetMasterPasswordHash(masterPassword, salt);

        return await _uow.Users
            .FirstOrDefaultAsync(x => x.Email == email && x.MasterPasswordHash == masterPasswordHash);
    }

    public async Task CreateNewUser(string email, string name, string masterPassword)
    {
        var salt = Crypto.Crypto.GetSalt();
        var key = Convert.ToBase64String(salt);
        var masterPasswordHash = Crypto.Crypto.GetMasterPasswordHash(masterPassword, salt);

        var user = new User
        {
            Email = email,
            Name = name,
            MasterPasswordHash = masterPasswordHash,
            Key = key
        };

        _uow.Users.Update(user);

        await _uow.SaveChangesAsync();
    }
}