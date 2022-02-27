using Piet.Color;

namespace Piet.Grid;

public sealed record Codel(int XPosition, int YPosition, IColor Color)
{
    public IColor Color { get; set; } = Color;
}