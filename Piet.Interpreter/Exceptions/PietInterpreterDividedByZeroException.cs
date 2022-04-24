namespace Piet.Interpreter.Exceptions;

internal class PietInterpreterDividedByZeroException : Exception
{
    public PietInterpreterDividedByZeroException(string message) : base(message)
    {
    }
}