using Domain.Entities;

namespace Core.Services.Interfaces;

public interface ISecretService
{
    Task<Secret> Save(Secret secret, Guid userId);
    Task Delete(Guid id, Guid userId);
}