namespace Piet.Interpreter.Output;

public sealed class ProgramOperatorUpdateEventArgs : EventArgs
{
    public Stack<int> CurrentInterpreterStack { get; set; }

    public ProgramOperatorUpdateEventArgs(Stack<int> currentInterpreterStack)
    {
        CurrentInterpreterStack = currentInterpreterStack;
    }
}