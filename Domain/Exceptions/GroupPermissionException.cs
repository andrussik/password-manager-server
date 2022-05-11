using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class GroupPermissionException : PermissionException
{
    public GroupPermissionException() { }
    public GroupPermissionException(string message) : base(message) { }
}