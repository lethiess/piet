using Dawn;
using Piet.Color;

namespace Piet.Grid;

public sealed class PietDataGridBuilder
{
    private int _height;
    private int _width;
    private PietColor? _initialColor;
    private bool _randomCellColors = false;

    public PietDataGridBuilder()
    {
    }

    public PietDataGridBuilder WithHeight(int height)
    {
        _height = height;
        return this;
    }

    public PietDataGridBuilder WithWidth(int width)
    {
        _width = width;
        return this;
    }

    public PietDataGridBuilder WithInitialColor(PietColor color)
    {
        _initialColor = color;
        return this;
    }

    public PietDataGridBuilder WithRandomCellColors()
    {
        _randomCellColors = true;
        return this;
    }

    public PietDataGrid Build()
    {
        Guard.Argument(_height, nameof(_height))
            .Positive();
        Guard.Argument(_height, nameof(_height))
            .Positive();
        
        var pietDataGrid = new PietDataGrid(_height, _width, _initialColor);

        if (_randomCellColors)
        {
            pietDataGrid.FillWithRandomValues();
        }

        return pietDataGrid;
    }
}