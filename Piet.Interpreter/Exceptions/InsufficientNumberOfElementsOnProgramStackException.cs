namespace Piet.Interpreter.Exceptions;

public class InsufficientNumberOfElementsOnProgramStackException : Exception
{
    public InsufficientNumberOfElementsOnProgramStackException(string message) : base(message)
    {
    }
}