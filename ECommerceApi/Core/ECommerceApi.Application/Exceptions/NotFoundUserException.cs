namespace ECommerceApi.Application.Exceptions;

public class NotFoundUserException : Exception
{
    public NotFoundUserException() : base("User not found")
    {
        
    }

    public NotFoundUserException(string? message) : base(message)
    {
        
    }

    public NotFoundUserException(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
}