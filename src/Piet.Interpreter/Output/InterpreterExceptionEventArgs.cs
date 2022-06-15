namespace Piet.Interpreter.Output;

public sealed class InterpreterExceptionEventArgs : EventArgs
{
    public string Message { get; set; }

    public InterpreterExceptionEventArgs(string message)
    {
        Message = message;
    }
}