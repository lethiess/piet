namespace Piet.Interpreter.Output;

public class OutputIntegerOperationEventArgs : EventArgs
{
    public int Value { get; set; }

    public OutputIntegerOperationEventArgs(int value)
    {
        Value = value;
    }
}