using Piet.Grid;

namespace Piet.Interpreter;

public interface ICodelChooser
{
    ICodelGrid CodelGrid { get; set; }
    CodelResult GetNextCodel(IEnumerable<Codel> currentCodelBlock);
}