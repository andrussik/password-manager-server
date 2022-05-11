using Core.Dtos;
using Core.Interfaces;
using Core.Mappers;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Core.Services;

public class SecretService : ISecretService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;

    public SecretService(IUnitOfWork uow, IUserService userService)
    {
        _uow = uow;
        _userService = userService;
    }

    public Task<Secret> Create(SecretDto secretDto) => Save(SecretsMapper.Map(secretDto));

    public Task<Secret> Update(Guid id, SecretDto secretDto)
    {
        var existingSecret = _uow.Secrets.FirstOrDefault(x => x.Id == id);
        
        if (existingSecret is null)
            throw new SecretException(RK.ERR_MSG_ENTITY_NOT_FOUND);
        
        var secret = SecretsMapper.Map(secretDto);
        secret.Id = id;

        return Save(existingSecret);
    }

    public async Task<Secret> Save(Secret secret)
    {
        secret.Validate();

        await ValidateWriting(secret);

        _uow.Secrets.Update(secret);

        await _uow.SaveChangesAsync();

        return secret;
    }

    public async Task Delete(Guid id)
    {
        var secret = await _uow.Secrets.FindAsync(id);

        if (secret is null)
            throw new SecretException(RK.ERR_MSG_ENTITY_NOT_FOUND);

        await ValidateWriting(secret);

        _uow.Secrets.Remove(secret);

        await _uow.SaveChangesAsync();
    }

    private async Task ValidateWriting(Secret secret)
    {
        if (secret.UserId is not null)
            secret.ValidateOwnership(_userService.CurrentUserId);
        else
        {
            var groupUser = await _uow.GroupUsers.FirstOrDefaultAsync(x => 
                    x.UserId == _userService.CurrentUserId && x.GroupId == secret.GroupId);

            if (groupUser is null)
                throw new SecretPermissionException("User does not belong to group.");

            if (!groupUser.WriteAllowed())
                throw new SecretPermissionException("User does not have permission to save secret in this group.");
        }
    }
}