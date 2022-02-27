using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Data;
using Core.Data.Repositories;
using Core.Services.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utilities;

namespace Core.Services;

public class TokenService : ITokenService
{
    private readonly IUnitOfWork _uow;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public TokenService(
        IUnitOfWork uow,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IConfiguration configuration,
        TokenValidationParameters tokenValidationParameters)
    {
        _uow = uow;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _tokenValidationParameters = tokenValidationParameters;
        _configuration = configuration;
    }

    public JwtSecurityToken GenerateJwtSecurityToken(User user)
    {
        var key = _tokenValidationParameters.IssuerSigningKey;

        return new JwtSecurityToken(
            _tokenValidationParameters.ValidIssuer,
            _tokenValidationParameters.ValidAudience,
            new Claim[]
            {
                new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            },
            expires: DateTime.Now.Add(_configuration.GetValue<TimeSpan>(ConfigurationKeys.JWT_TOKEN_LIFETIME)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
        );
    }

    public async Task<RefreshToken> GenerateRefreshToken(string jwtId, Guid userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        while (await _refreshTokenRepository.Exists(token))
        {
            token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        var refreshToken = new RefreshToken
        {
            JwtId = jwtId,
            IsUsed = false,
            IsRevoked = false,
            UserId = userId,
            AddedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(1),
            Token = token
        };

        _refreshTokenRepository.Update(refreshToken);

        await _uow.SaveChanges();

        return refreshToken;
    }

    public async Task<JwtSecurityToken?> RefreshToken(string token, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                    StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                    return null;
            }

            var expiredAtUnix =
                long.Parse(principal.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiredAtUtc = DateTime.UnixEpoch.AddSeconds(expiredAtUnix);

            if (expiredAtUtc > DateTime.UtcNow)
            {
                throw new Exception("This token hasn't expired yet.");
            }

            var storedRefreshToken = await _refreshTokenRepository.FirstOrDefault(x => x.Token == refreshToken);
            if (storedRefreshToken is null)
                throw new Exception("Token does not exist.");

            if (storedRefreshToken.IsUsed)
                throw new Exception("Token has been used.");

            if (storedRefreshToken.IsRevoked)
                throw new Exception("Token has been revoked.");

            var jti = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (storedRefreshToken.JwtId != jti)
                throw new Exception("Token does not match JWT.");

            storedRefreshToken.IsUsed = true;
            _refreshTokenRepository.Update(storedRefreshToken);
            await _uow.SaveChanges();

            var user = await _userRepository.Get(storedRefreshToken.UserId);

            return GenerateJwtSecurityToken(user);
        }
        catch (Exception) { }

        return null;
    }
}