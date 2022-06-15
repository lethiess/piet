namespace Piet.Interpreter;

public class Context
{
    public Action? Pause { get; init; }
    public Action? OnError { get; init; }
}