using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class RegisterDto
{
    [Required]
    [MinLength(5)]
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string MasterPassword { get; set; } = default!;
}