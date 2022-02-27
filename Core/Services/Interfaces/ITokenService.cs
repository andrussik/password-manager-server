using System.IdentityModel.Tokens.Jwt;
using Domain.Entities;

namespace Core.Services.Interfaces;

public interface ITokenService
{
    JwtSecurityToken GenerateJwtSecurityToken(User user);
    Task<RefreshToken> GenerateRefreshToken(string jwtId, Guid userId);
    Task<JwtSecurityToken?> RefreshToken(string token, string refreshToken);
}