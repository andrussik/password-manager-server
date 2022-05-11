namespace Domain.Entities.Base;

public class EntityWithMeta : Entity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Guid? UpdatedBy { get; set; }
}