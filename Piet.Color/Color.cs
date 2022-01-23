using System.ComponentModel;

namespace Piet.Color;

internal abstract class Color : IColor
{
    public int R { get; protected set; }
    public int G { get; protected set; }
    public int B { get; protected set; }
    [DefaultValueAttribute(1.0)]
    public double Alpha { get; protected set; }
}
