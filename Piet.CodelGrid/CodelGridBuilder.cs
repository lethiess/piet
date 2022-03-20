using Dawn;
using Piet.Color;

namespace Piet.Grid;

public sealed class CodelGridBuilder
{
    private int _height;
    private int _width;
    private PietColor? _initialColor;
    private bool _randomCellColors = false;
    public CodelGridBuilder WithHeight(int height)
    {
        _height = height;
        return this;
    }

    public CodelGridBuilder WithWidth(int width)
    {
        _width = width;
        return this;
    }

    public CodelGridBuilder WithInitialColor(PietColor color)
    {
        _initialColor = color;
        return this;
    }

    public CodelGridBuilder WithRandomCellColors()
    {
        _randomCellColors = true;
        return this;
    }

    public CodelGrid Build()
    {
        Guard.Argument(_height, nameof(_height))
            .Positive();
        Guard.Argument(_height, nameof(_height))
            .Positive();
        
        var pietDataGrid = new CodelGrid(_height, _width);

        if (_randomCellColors)
        {
            pietDataGrid.FillWithRandomValues();
        }

        return pietDataGrid;
    }
}