using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Key), nameof(CultureId), IsUnique = true)]
public class Resource : Entity
{
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;

    public Guid CultureId { get; set; }
    public Culture? Culture { get; set; }
}