using Piet.Grid;

namespace Piet.Interpreter;

public interface ICodelChooser
{
    Codel GetNextCodel(IEnumerable<Codel> currentCodelBlock);
}