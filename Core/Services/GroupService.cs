using Core.Data;
using Core.Data.Repositories;
using Core.Services.Interfaces;
using Domain.Entities;

namespace Core.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IGroupUserRepository _groupUserRepository;
    private readonly IUnitOfWork _uow;
    
    public GroupService(IGroupRepository groupRepository, IGroupUserRepository groupUserRepository, IUnitOfWork uow)
    {
        _groupRepository = groupRepository;
        _groupUserRepository = groupUserRepository;
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

    public async Task Delete(Guid id, Guid userId)
    {
        var group = await _groupRepository.FirstOrDefault(x => x.Id == id);

        if (group is null)
            throw new Exception("Group with given ID not found.");

        var groupUser = 
            await _groupUserRepository.FirstOrDefault(x => x.GroupId == group.Id && x.UserId == userId);

        if (groupUser is null)
            throw new Exception("User is not in group.");

        if (groupUser.GroupRoleId != GroupRole.Owner.Id)
            throw new Exception("Only group owner has permission to delete group.");

        var groupUsers = await _groupUserRepository.Search(x => x.GroupId == group.Id);

        await _uow.BeginTransaction();
        
        
        _groupUserRepository.RemoveRange(groupUsers);
        
        await _uow.SaveChanges();
        
        
        _groupRepository.Remove(group);

        await _uow.SaveChanges();
        

        await _uow.Commit();
    }
}