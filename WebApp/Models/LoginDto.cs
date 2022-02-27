namespace WebApp.Models;

public class LoginDto
{
    public string Email { get; set; } = default!;
    public string MasterPassword { get; set; } = default!;
}