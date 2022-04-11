namespace Piet.Interpreter;

public class InsufficientNumberOfElementsOnProgramStackException : Exception
{
    public InsufficientNumberOfElementsOnProgramStackException(string message) : base(message)
    {
    }
}