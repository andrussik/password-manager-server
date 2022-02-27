namespace Domain.Entities.Base;

public abstract class SecretOwner : Entity
{
    public string Key { get; set; } = default!;

    public IList<Secret>? Secrets { get; set; }
}