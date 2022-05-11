using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Code), IsUnique = true)]
public class Culture : Entity
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;

    public ICollection<Resource>? Resources { get; set; }

    public static readonly Culture Estonian = new()
    {
        Id = new Guid("044da860-268b-44df-b171-09e9238bcd48"),
        Code = "et-EE", 
        Name = "Estonian (Estonia)"
    };
    
    public static readonly Culture English = new()
    {
        Id = new Guid("4cfb2a30-98da-48ea-b97f-6fe28ee64c91"),
        Code = "en-GB", 
        Name = "English (United Kingdom)"
    };
}