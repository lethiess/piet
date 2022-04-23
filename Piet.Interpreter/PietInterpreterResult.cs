namespace Piet.Interpreter;

public sealed record PietInterpreterResult(PietInterpreterResult.InterpreterStatus Status, string? Message)
{
    public enum InterpreterStatus
    {
        Success,
        Error
    }
};