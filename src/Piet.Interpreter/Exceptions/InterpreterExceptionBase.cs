namespace Piet.Interpreter.Exceptions;

public class InterpreterExceptionBase: Exception
{
    public InterpreterExceptionBase(string message) : base(message)
    {
    }
}