using Core.Data;
using Core.Data.Repositories;
using Core.Services.Interfaces;
using Domain.Entities;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ISecretRepository _secretRepository;
    private readonly IGroupUserRepository _groupUserRepository;
    private readonly IUnitOfWork _uow;

    public UserService(
        IUserRepository userRepository,
        ISecretRepository secretRepository,
        IGroupUserRepository groupUserRepository,
        IUnitOfWork uow)
    {
        _userRepository = userRepository;
        _secretRepository = secretRepository;
        _groupUserRepository = groupUserRepository;
        _uow = uow;
    }

    public async Task<User> Get(Guid id)
    {
        var user = await _userRepository.Get(id);

        var userSecrets = await _secretRepository.Search(x => x.UserId == id);
        user.Secrets = userSecrets.ToList();

        var userGroups = await _groupUserRepository.SearchAndIncludeGroupSecrets(x => x.UserId == id);
        user.GroupUsers = userGroups;

        return user;
    }

    public async Task<User?> Get(string email, string masterPassword)
    {
        var userKey = await _userRepository.GetUserKey(email);
        if (userKey is null)
            return null;

        var salt = Convert.FromBase64String(userKey);

        var masterPasswordHash = Crypto.Crypto.GetMasterPasswordHash(masterPassword, salt);

        return await _userRepository.FirstOrDefault(x =>
            x.Email == email && x.MasterPasswordHash == masterPasswordHash);
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

        _userRepository.Update(user);

        await _uow.SaveChanges();
    }
}