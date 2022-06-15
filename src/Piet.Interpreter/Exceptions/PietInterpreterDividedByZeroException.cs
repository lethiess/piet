namespace Piet.Interpreter.Exceptions;

internal class PietInterpreterDividedByZeroException : InterpreterExceptionBase
{
    public PietInterpreterDividedByZeroException(string message) : base(message)
    {
    }
}