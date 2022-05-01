using Piet.Command;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter;

public interface IProgramOperator
{
    IInputFacade InputFacade { get; }
    IOutputService OutputService { get; }
    void ExecuteCommand(ColorCommand colorCommand, int codelBlockSize);
}