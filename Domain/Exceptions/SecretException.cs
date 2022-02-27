using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class SecretException : BusinessLogicException
{
    public SecretException(string message) : base(message) { }
}