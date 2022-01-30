using Dawn;
using Piet.Color;

namespace Piet.Grid;
public sealed class PietDataGrid
{
    private readonly PietColor[,] _girdData;

    public int Height { get; init; }
    public int Width { get; init; }

    public PietDataGrid(int height, int width)
    {
        Height = height;
        Width = width;
        _girdData = new PietColor[height, width];
    }

    public void FillWithRandomValues()
    {
        Random random = new Random();
        for (int yPosition = 0; yPosition < Height; yPosition++)
        {
            for (int xPosition = 0; xPosition < Width; xPosition++)
            {
                _girdData[yPosition, xPosition] = new PietColor().GetRandomColor();
            }
        }
    }

    public void SetRandomCellColor(int yPosition, int xPosition)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Height - 1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Width - 1);

        Random random = new Random();
        _girdData[yPosition, xPosition] = new PietColor().GetRandomColor();
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
            .InRange(0, Height - 1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Width - 1);

        _girdData[yPosition, xPosition] = color;
    }

    public PietColor GetCell(int yPosition, int xPosition)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Height - 1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Width - 1);

        return _girdData[yPosition, xPosition];
    }

}
