using Piet.Command;
using Piet.Interpreter.Events;

namespace Piet.Interpreter;

public interface IProgramOperator
{
    IInputService InputService { get; }
    IOutputService OutputService { get; }
    void ExecuteCommand(ColorCommand colorCommand, int codelBlockSize);
}