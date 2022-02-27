using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class GroupDto
{
    public Guid Id { get; set; }
    
    [MinLength(2)]
    public string Name { get; set; } = default!;
}