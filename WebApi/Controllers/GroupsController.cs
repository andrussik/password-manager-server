using Core.Dtos;
using Core.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi.Controllers;

public class GroupsController : BaseController
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public async Task<UserGroupDto> Save(GroupDto group)
        => new((await _groupService.Create(group.Name)).GroupUsers!.First());
    
    [HttpPatch("{id:guid}")]
    public async Task Patch(Guid id, JsonPatchDocument<Group> patchDocument) 
        => await _groupService.Update(id, patchDocument);

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id) => await _groupService.Delete(id);
}