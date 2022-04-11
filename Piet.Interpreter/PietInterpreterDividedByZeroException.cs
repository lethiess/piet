namespace Piet.Interpreter;

internal class PietInterpreterDividedByZeroException : Exception
{
    public PietInterpreterDividedByZeroException(string message) : base(message)
    {
    }
}