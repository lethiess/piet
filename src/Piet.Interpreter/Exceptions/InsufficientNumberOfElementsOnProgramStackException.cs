namespace Piet.Interpreter.Exceptions;

public class InsufficientNumberOfElementsOnProgramStackException : InterpreterExceptionBase
{
    public InsufficientNumberOfElementsOnProgramStackException(string message) : base(message)
    {
    }
}