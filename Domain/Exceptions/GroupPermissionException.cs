using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class GroupPermissionException : PermissionException
{
    public GroupPermissionException(string message) : base(message) { }
}