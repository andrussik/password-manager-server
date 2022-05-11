using System.ComponentModel.DataAnnotations;

namespace Core.Dtos;

public class LoginDto
{
    [EmailAddress]
    public string Email { get; set; } = default!;

    public string MasterPassword { get; set; } = default!;
}