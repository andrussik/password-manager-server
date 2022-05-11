namespace Core.Dtos;

public class RefreshTokenRequest
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}