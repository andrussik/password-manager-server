using Core.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities;

namespace Core.Services;

public class GroupService : IGroupService
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;
    private readonly ILogger<GroupService> _log;

    public GroupService(IUnitOfWork uow, IUserService userService, ILogger<GroupService> log)
    {
        _uow = uow;
        _userService = userService;
        _log = log;
    }

    public async Task<Group> Create(string name)
    {
        var group = new Group
        {
            Name = name,
            Key = Convert.ToBase64String(Crypto.Crypto.GetSalt())
        };

        var groupOwner = new GroupUser
        {
            UserId = _userService.CurrentUserId,
            GroupRoleId = GroupRole.Owner.Id
        };

        group.GroupUsers = new List<GroupUser> { groupOwner };

        _uow.Groups.Update(group);
        await _uow.SaveChangesAsync();

        return group;
    }

    public async Task Update(Guid id, JsonPatchDocument<Group> patchDocument)
    {
        var group = await _uow.Groups.FirstOrDefaultAsync(x => x.Id == id);

        if (group is null)
            throw new GroupException(RK.ERR_MSG_ENTITY_NOT_FOUND);

        var groupUser = await _uow.GroupUsers.FirstOrDefaultAsync(x =>
            x.GroupId == group.Id && x.UserId == _userService.CurrentUserId);

        if (groupUser is null)
            throw new GroupPermissionException();

        foreach (var operation in patchDocument.Operations)
        {
            if (!operation.path.Equals(nameof(Group.Name), StringComparison.OrdinalIgnoreCase))
                throw new GroupPermissionException();

            if (operation.path.Equals(nameof(Group.Name), StringComparison.OrdinalIgnoreCase) 
                && !groupUser.UpdateNameAllowed())
                throw new GroupPermissionException();
        }

        patchDocument.ApplyTo(group);

        await _uow.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var group = await _uow.Groups.FirstOrDefaultAsync(x => x.Id == id);

        await Validate(group);
        await ValidateOwnership(group!.Id);

        var groupSecrets = await _uow.Secrets.Where(x => x.GroupId == group.Id).ToListAsync();
        var groupUsers = await _uow.GroupUsers.Where(x => x.GroupId == group.Id).ToListAsync();

        await _uow.BeginTransactionAsync();

        _uow.Secrets.RemoveRange(groupSecrets);
        await _uow.SaveChangesAsync();

        _uow.GroupUsers.RemoveRange(groupUsers);
        await _uow.SaveChangesAsync();

        _uow.Groups.Remove(group);
        await _uow.SaveChangesAsync();

        await _uow.CommitAsync();
    }

    private async Task Validate(Group? group)
    {
        if (group is null)
            throw new GroupException("Group with given ID not found.");

        var groupUser = await _uow.GroupUsers.FirstOrDefaultAsync(x =>
            x.GroupId == group.Id && x.UserId == _userService.CurrentUserId);

        if (groupUser is null)
            throw new GroupPermissionException("User is not in group.");
    }

    private async Task ValidateOwnership(Guid groupId)
    {
        var groupUser = await _uow.GroupUsers.FirstOrDefaultAsync(x =>
            x.GroupId == groupId && x.UserId == _userService.CurrentUserId);

        if (groupUser is null)
            throw new GroupPermissionException("User is not in group.");

        if (groupUser.GroupRoleId != GroupRole.Owner.Id)
            throw new GroupPermissionException("User is not owner of this group.");
    }
}