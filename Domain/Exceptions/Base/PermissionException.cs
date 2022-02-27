namespace Domain.Exceptions.Base;

public class PermissionException : Exception
{
    protected PermissionException(string message) : base(message) { }
}