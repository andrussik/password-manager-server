namespace Core.Dtos;

public class RegisterDto
{
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string MasterPassword { get; set; } = default!;
}