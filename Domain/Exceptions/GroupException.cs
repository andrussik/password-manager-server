using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public class GroupException : BusinessLogicException
{
    public GroupException(string message) : base(message) { }
}