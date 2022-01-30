using Dawn;
using Piet.Color;

namespace Piet.Grid;
public sealed class PietDataGrid
{
    private readonly PietColor[,] _girdData;
    private readonly PietColor _defaultColor = PietColors.White;
    public int Height { get; init; }
    public int Width { get; init; }

    private void InitializeGrid()
    {
        SetUniqueGridColor(_defaultColor);
    }
    public PietDataGrid(int height, int width)
    {
        Height    = height;
        Width     = width;
        _girdData = new PietColor[height, width];
        InitializeGrid();
    }

    public void FillWithRandomValues()
    {
        Random random = new Random();
        for (int yPosition = 0; yPosition < Height; yPosition++)
        {
            for (int xPosition = 0; xPosition < Width; xPosition++)
            {
                _girdData[yPosition, xPosition] =
                    PietColorFactory.CreateRandomColor();
            }
        }
    }

    public void SetRandomCellColor(int yPosition, int xPosition)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height);

        Random random = new Random();
        _girdData[yPosition, xPosition] = PietColorFactory.CreateRandomColor();
    }

    public void SetUniqueGridColor(PietColor color)
    {
        for (int yPosition = 0; yPosition < Height; yPosition++)
        {
            for (int xPosition = 0; xPosition < Width; xPosition++)
            {
                _girdData[yPosition, xPosition] = color;
            }
        }
    }

    public void SetCellColor(int yPosition, int xPosition, PietColor color)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height);

        _girdData[yPosition, xPosition] = color;
    }

    public PietColor GetCell(int yPosition, int xPosition)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height);

        return _girdData[yPosition, xPosition];
    }
}
