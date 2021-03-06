using Piet.Command;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter;

public interface IProgramOperator
{
    IInputService InputService { get; }
    IOutputService OutputService { get; }
    void SetInputValue(int input);
    void ResetProgramStack();
    void ExecuteCommand(ColorCommand colorCommand, int codelBlockSize, Context context);
}