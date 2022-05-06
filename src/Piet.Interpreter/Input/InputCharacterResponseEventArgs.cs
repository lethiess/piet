namespace Piet.Interpreter.Input;

public sealed class InputCharacterResponseEventArgs : EventArgs
{
    public char Value { get; set; }

    public InputCharacterResponseEventArgs(char value)
    {
        Value = value;
    }
}