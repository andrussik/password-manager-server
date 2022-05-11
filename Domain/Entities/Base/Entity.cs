namespace Domain.Entities.Base;

public abstract class Entity
{
    public Guid Id { get; set; }

    public bool IsNew() => Id == default;
}