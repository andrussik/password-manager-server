using Domain.Entities;

namespace Core.Services.Interfaces;

public interface IGroupService
{
    Task<Group> Create(string name, Guid userId);
    Task Delete(Guid id, Guid userId);
}