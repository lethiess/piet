using Piet.Grid;

namespace Piet.Interpreter;

public interface ICodelBlockSearcher
{
    ICodelGrid CodelGrid { get; set; }
    IEnumerable<Codel> GetCodelBock(Codel currentCodel);
}