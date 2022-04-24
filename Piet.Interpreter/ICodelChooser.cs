using Piet.Grid;

namespace Piet.Interpreter;

public interface ICodelChooser
{
    ICodelGrid CodelGrid { get; init; }
    CodelResult GetNextCodel(IEnumerable<Codel> currentCodelBlock);
}