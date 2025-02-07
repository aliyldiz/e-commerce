namespace ECommerceApi.Application.Exceptions;

public class PasswordChangeFailedException : Exception
{
    public PasswordChangeFailedException() : base("Password change failed!")
    {
        
    }

    public PasswordChangeFailedException(string? message) : base(message)
    {
        
    }

    public PasswordChangeFailedException(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
}