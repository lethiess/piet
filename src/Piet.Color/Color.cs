namespace Piet.Color;

public record Color : IColor
{
    public int R { get; protected set; }
    public int G { get; protected set; }
    public int B { get; protected set; }
    public double Alpha { get; protected set; } = 1.0;
}
