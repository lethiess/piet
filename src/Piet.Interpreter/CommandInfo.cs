using Piet.Command;

namespace Piet.Interpreter;

public sealed record CommandInfo
{
    public ColorCommand ColorCommand { get; set; }
    public int? Value { get; set; }
    public int? OperandA { get; set; }
    public int? OperandB { get; set; }

    public CommandInfo(
        ColorCommand colorCommand, 
        int? value
        )
    {
        ColorCommand = colorCommand;
        Value = value;
    }
}