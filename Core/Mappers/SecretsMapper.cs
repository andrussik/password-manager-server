using Core.Dtos;
using Domain.Entities;

namespace Core.Mappers;

public static class SecretsMapper
{
    public static Secret Map(SecretDto secretDto)
    {
        return new Secret
        {
            Id = secretDto.Id,
            Name = secretDto.Name,
            Username = secretDto.Username,
            Password = secretDto.Password,
            Description = secretDto.Description,
            UserId = secretDto.UserId,
            GroupId = secretDto.GroupId
        };
    }
    
    public static SecretDto Map(Secret secret)
    {
        return new SecretDto
        {
            Id = secret.Id,
            Name = secret.Name,
            Username = secret.Username,
            Password = secret.Password,
            Description = secret.Description,
            UserId = secret.UserId,
            GroupId = secret.GroupId
        };
    }
}