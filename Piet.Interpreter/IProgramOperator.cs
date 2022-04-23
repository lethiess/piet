using Piet.Command;

namespace Piet.Interpreter;

public interface IProgramOperator
{
    void ExecuteCommand(ColorCommand colorCommand, int codelBlockSize);
}