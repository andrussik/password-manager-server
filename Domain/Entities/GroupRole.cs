using Domain.Entities.Base;

namespace Domain.Entities;

public class GroupRole : Entity
{
    public string Name { get; set; } = default!;
    
    public static readonly GroupRole Owner = new ()
    {
        Id = new Guid("0813fc0a-0719-4ea1-b99a-e46f50574e0b"),
        Name = "Owner"
    };
    
    public static readonly GroupRole Admin = new ()
    {
        Id = new Guid("cfecfc02-da76-45eb-8eda-bde7bb03c738"),
        Name = "Admin"
    };
    
    public static readonly GroupRole Writer = new ()
    {
        Id = new Guid("7e8edd0e-be29-4fa7-aba8-3031423a4d7f"),
        Name = "Writer"
    };
    
    public static readonly GroupRole Reader = new ()
    {
        Id = new Guid("e02a0e63-1474-4c68-b16f-5692c75bc347"),
        Name = "Reader"
    };

    public static string GetName(Guid id) => id switch
    {
        var value when value == Owner.Id => Owner.Name,
        var value when value == Admin.Id => Admin.Name,
        var value when value == Writer.Id => Writer.Name,
        var value when value == Reader.Id => Reader.Name,
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
    };
}