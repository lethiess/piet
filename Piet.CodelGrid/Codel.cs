using Piet.Color;

namespace Piet.Grid;

public sealed record Codel(int XPosition, int YPosition, PietColor Color)
{
    public PietColor Color { get; set; } = Color;
}