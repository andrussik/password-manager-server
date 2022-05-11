using Utilities;

namespace Domain.Exceptions.Base;

public abstract class PermissionException : Exception
{
    protected PermissionException() : base(RS.Get(RK.ERR_MSG_NO_PERMISSION)) { }
    protected PermissionException(string? message) : base(message) { }
}