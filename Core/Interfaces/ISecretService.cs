using Core.Dtos;
using Domain.Entities;

namespace Core.Interfaces;

public interface ISecretService
{
    Task<Secret> Create(SecretDto secretDto);
    Task<Secret> Update(Guid id, SecretDto secretDto);
    Task<Secret> Save(Secret secret);
    Task Delete(Guid id);
}