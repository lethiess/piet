using Piet.Grid;

namespace Piet.Interpreter;

public interface ICodelChooser
{
    ICodelGrid CodelGrid { get; init; }
    Codel GetNextCodel(IEnumerable<Codel> currentCodelBlock);
}