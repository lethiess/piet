namespace Piet.Color;
public sealed record PietColor : Color
{
    public PietColorNames Name { get; }

    internal PietColor(int red, int green, int blue, PietColorNames name)
    {
        Name = name;
        R = red;
        G = green;
        B = blue;
    }
}