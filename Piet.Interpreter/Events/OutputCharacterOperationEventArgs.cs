namespace Piet.Interpreter.Events;

public class OutputCharacterOperationEventArgs : EventArgs
{
    public char Value { get; set; }

    public OutputCharacterOperationEventArgs(char value)
    {
        Value = value;
    }
}