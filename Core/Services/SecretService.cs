using Core.Data;
using Core.Data.Repositories;
using Core.Services.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Core.Services;

public class SecretService : ISecretService
{
    private readonly ISecretRepository _secretRepository;
    private readonly IGroupUserRepository _groupUserRepository;
    private readonly IUnitOfWork _uow;

    public SecretService(ISecretRepository secretRepository, IGroupUserRepository groupUserRepository, IUnitOfWork uow)
    {
        _secretRepository = secretRepository;
        _groupUserRepository = groupUserRepository;
        _uow = uow;
    }

    public async Task<Secret> Save(Secret secret, Guid userId)
    {
        secret.Validate();
        
        await ValidateWriting(secret, userId);

        _secretRepository.Update(secret);

        await _uow.SaveChanges();

        return secret;
    }

    public async Task Delete(Guid id, Guid userId)
    {
        var secret = await _secretRepository.Get(id);

        await ValidateWriting(secret, userId);

        _secretRepository.Remove(secret);

        await _uow.SaveChanges();
    }

    private async Task ValidateWriting(Secret secret, Guid userId)
    {
        if (secret.UserId is not null)
            secret.ValidateOwnership(userId);
        else
        {
            var groupUser =
                await _groupUserRepository.FirstOrDefault(x => x.UserId == userId && x.GroupId == secret.GroupId);

            if (groupUser is null)
                throw new SecretPermissionException("User does not belong to group.");

            if (!groupUser.WriteAllowed())
                throw new SecretPermissionException("User does not have permission to save secret in this group.");
        }
    }
}