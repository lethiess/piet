using Piet.Grid;

namespace Piet.Interpreter;

public sealed record CodelResult
{
    public Codel? Codel { get; set; } = null;
    public bool TraversedWhiteCodels { get; set; } = false;
    public bool Success { get; set; } = false;
}