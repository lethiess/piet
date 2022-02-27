using Piet.Grid;

namespace Piet.Interpreter;

internal sealed class CodelChooser : ICodelChooser
{
    private readonly CodelGrid _codelGrid;
    private Direction _directionPointer;

    public CodelChooser(
        CodelGrid codelGrid)
    {
        _codelGrid = codelGrid;
        _directionPointer = Direction.Right;
    }

    public Codel GetNextCodel(IEnumerable<Codel> currentCodelBock)
    {
        // todo: determine next codel

        throw new NotImplementedException();
    }
}