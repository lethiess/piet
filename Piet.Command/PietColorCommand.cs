using Piet.Color;

namespace Piet.Command;

public record PietColorCommand(
    PietColor Color,
    Command Command)
{
}