using Domain.Entities.Base;

namespace Domain.Entities;

public class GroupInvitation : Entity
{
    public DateTime InvitedAt { get; set; }
    
    public Guid InvitedUserId { get; set; }
    public User? InvitedUser { get; set; }

    public Guid InvitedByUserId { get; set; }
    public User? InvitedByUser { get; set; }
}