using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace Core.Interfaces;

public interface IGroupService
{
    Task<Group> Create(string name);
    Task Update(Guid id, JsonPatchDocument<Group> patchDocument);
    Task Delete(Guid id);
}