using Piet.Grid;

namespace Piet.Interpreter;

public interface ICodelBlockSearcher
{
    void Initialize(ICodelGrid codelGrid);
    IEnumerable<Codel> GetCodelBock(Codel currentCodel);
}