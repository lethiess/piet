namespace Piet.Color;

public class InvalidPietColorCodeException : Exception
{
    public InvalidPietColorCodeException(string message)
        : base(message)
    {
    }
}