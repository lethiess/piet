using Piet.Command;
using Piet.Grid;

namespace Piet.Interpreter;

public sealed class PietInterpreter
{
    private readonly CodelGrid _codelGrid;
    private readonly CodelChooser _chooser;
    private readonly DirectionPointer _directionPointer;

    public PietInterpreter(
        CodelGrid codelGrid,
        CodelChooser chooser,
        DirectionPointer directionPointer
)
    {
        _codelGrid = codelGrid;
        _chooser = chooser;
        _directionPointer = directionPointer;
    }
}