using System.IdentityModel.Tokens.Jwt;
using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AccountsController : BaseController
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AccountsController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }
    
    // [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login(LoginDto loginDto)
    {
        var user = await _userService.Get(loginDto.Email, loginDto.MasterPassword);

        if (user is null)
            return Unauthorized();

        var jwtSecurityToken = _tokenService.GenerateJwtSecurityToken(user);
        
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        var refreshToken = await _tokenService.GenerateRefreshToken(jwtSecurityToken.Id, user.Id);

        return Ok(new TokenResponse { Token = jwtToken, RefreshToken = refreshToken.Token });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        await _userService.CreateNewUser(registerDto.Email, registerDto.Name, registerDto.MasterPassword);

        return Ok();
    }

    [HttpGet("token")]
    public ActionResult<TokenResponse> GetToken() => Ok(true);

    [AllowAnonymous]
    [HttpGet("refresh-token")]
    public async Task<ActionResult<TokenResponse>> GetRefreshToken(RefreshTokenRequest tokenRequest)
    {
        var jwtSecurityToken = await _tokenService.RefreshToken(tokenRequest.Token, tokenRequest.RefreshToken);
        
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        
        return Ok(new TokenResponse { Token = jwtToken, RefreshToken = tokenRequest.RefreshToken });
    }
}