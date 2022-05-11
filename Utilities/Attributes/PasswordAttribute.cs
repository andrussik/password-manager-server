using System.ComponentModel.DataAnnotations;

namespace Utilities.Attributes;

public class PasswordAttribute : RegularExpressionAttribute
{
    public PasswordAttribute() : base("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$") { }
}