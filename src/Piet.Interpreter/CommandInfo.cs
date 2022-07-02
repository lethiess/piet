using System.Collections.Immutable;
using Piet.Command;
using Piet.Grid;

namespace Piet.Interpreter;

public sealed record CommandInfo
{
    public ColorCommand ColorCommand { get; set; }
    public int? Value { get; set; }
    public int? OperandA { get; set; }
    public int? OperandB { get; set; }

    public ImmutableList<Codel> CodelBlock { get; set; }

    public CommandInfo(
        ColorCommand colorCommand,
        ImmutableList<Codel> codelBlock)
    {
        ColorCommand = colorCommand;
        CodelBlock = codelBlock;
    }
}