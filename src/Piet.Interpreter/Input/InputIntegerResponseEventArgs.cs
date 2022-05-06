namespace Piet.Interpreter.Input;

public sealed class InputIntegerResponseEventArgs: EventArgs
{
    public int Value { get; set; }

    public InputIntegerResponseEventArgs(int value)
    {
        Value = value;
    }
}