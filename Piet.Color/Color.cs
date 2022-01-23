namespace Piet.Color
{
    internal abstract class Color : IColor
    {
        public int R { get; init; }
        public int G { get; init; }
        public int B { get; init; }
        public double Alpha { get; init; }

    }
}