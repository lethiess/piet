using Piet.Color;

namespace Piet.Command;

public record ColorCommand(
    PietColor Color,
    Command Command)
{
}