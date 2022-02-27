namespace Domain.Exceptions.Base;

public abstract class BusinessLogicException : Exception
{
    protected BusinessLogicException(string message) : base(message) { }
}