namespace Piet.Color;
public sealed record PietColor : Color
{
    internal PietColor(int red, int green, int blue)
    {
        R = red;
        G = green;
        B = blue;
    }
}