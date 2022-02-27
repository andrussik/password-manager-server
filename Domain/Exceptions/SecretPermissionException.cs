using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class SecretPermissionException : PermissionException
{
    public SecretPermissionException(string message) : base(message) { }
}