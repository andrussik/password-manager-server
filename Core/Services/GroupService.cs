using Core.Data;
using Core.Data.Repositories;
using Core.Services.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Core.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IGroupUserRepository _groupUserRepository;
    private readonly ISecretRepository _secretRepository;
    private readonly IUnitOfWork _uow;

    public GroupService(
        IGroupRepository groupRepository,
        IGroupUserRepository groupUserRepository,
        ISecretRepository secretRepository,
        IUnitOfWork uow)
    {
        _groupRepository = groupRepository;
        _groupUserRepository = groupUserRepository;
        _secretRepository = secretRepository;
        _uow = uow;
    }

    public async Task<Group> Create(string name, Guid userId)
    {
        var group = new Group
        {
            Name = name,
            Key = Convert.ToBase64String(Crypto.Crypto.GetSalt())
        };

        var groupOwner = new GroupUser
        {
            UserId = userId,
            GroupRoleId = GroupRole.Owner.Id
        };

        group.GroupUsers = new List<GroupUser> { groupOwner };

        _groupRepository.Update(group);
        await _uow.SaveChanges();

        return group;
    }

    public async Task UpdateName(Guid id, string name, Guid userId)
    {
        var group = await _groupRepository.FirstOrDefault(x => x.Id == id);

        await Validate(group, userId);
        await ValidateOwnership(group!.Id, userId);

        group.Name = name;

        await _uow.SaveChanges();
    }

    public async Task Delete(Guid id, Guid userId)
    {
        var group = await _groupRepository.FirstOrDefault(x => x.Id == id);

        await Validate(group, userId);
        await ValidateOwnership(group!.Id, userId);

        var groupSecrets = await _secretRepository.Search(x => x.GroupId == group.Id);
        var groupUsers = await _groupUserRepository.Search(x => x.GroupId == group.Id);

        await _uow.BeginTransaction();

        _secretRepository.RemoveRange(groupSecrets);
        await _uow.SaveChanges();

        _groupUserRepository.RemoveRange(groupUsers);
        await _uow.SaveChanges();
        
        _groupRepository.Remove(group);
        await _uow.SaveChanges();
        
        await _uow.Commit();
    }

    private async Task Validate(Group? group, Guid userId)
    {
        if (group is null)
            throw new GroupException("Group with given ID not found.");

        var groupUser =
            await _groupUserRepository.FirstOrDefault(x => x.GroupId == group.Id && x.UserId == userId);

        if (groupUser is null)
            throw new GroupPermissionException("User is not in group.");
    }

    private async Task ValidateOwnership(Guid groupId, Guid userId)
    {
        var groupUser =
            await _groupUserRepository.FirstOrDefault(x => x.GroupId == groupId && x.UserId == userId);

        if (groupUser is null)
            throw new GroupPermissionException("User is not in group.");

        if (groupUser.GroupRoleId != GroupRole.Owner.Id)
            throw new GroupPermissionException("User is not owner of this group.");
    }
}