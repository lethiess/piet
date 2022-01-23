namespace Piet.Color
{
    internal interface IColor 
    {
        int R { get; init; }
        int G { get; init; }
        int B { get; init; }
        double Alpha { get; init; }
    }
}