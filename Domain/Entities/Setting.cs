using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Key), IsUnique = true)]
public class Setting : Entity
{
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
}