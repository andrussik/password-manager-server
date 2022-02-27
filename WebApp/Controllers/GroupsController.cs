using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class GroupsController : BaseController
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpPost]
    public async Task<UserGroupDto> Save(GroupDto group)
        => new((await _groupService.Create(group.Name, GetUserId())).GroupUsers!.First());

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task Delete(Guid id) => await _groupService.Delete(id, GetUserId());
}