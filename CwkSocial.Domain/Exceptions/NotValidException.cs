

namespace CwkSocial.Domain.Exceptions;

public class NotValidException : Exception
{
    public List<string> ValidationErrors { get; init; }

    internal NotValidException()
    {
        ValidationErrors = [];
    }

    internal NotValidException(string message) : base(message)
    {
        ValidationErrors = [];
    }

    internal NotValidException(string message, Exception inner) : base(message, inner)
    {
        ValidationErrors = [];
    }
}
