using Dawn;
using Piet.Color;

namespace Piet.Grid;

public sealed class CodelGrid : ICodelGrid
{
    private readonly Codel[,] _codelGrid;
    private readonly PietColor _initialColor;
    public int Height { get; init; }
    public int Width { get; init; }

    private void InitializeGrid()
    {
        for (int yPosition = 0; yPosition < Height; yPosition++)
        {
            for (int xPosition = 0; xPosition < Width; xPosition++)
            {
                _codelGrid[yPosition, xPosition] = new Codel(xPosition, yPosition, _initialColor);
            }
        }
    }
    public CodelGrid(int height, int width, PietColor? initialColor)
    {
        Height    = height;
        Width     = width;
        _initialColor = initialColor ?? PietColors.White;
        _codelGrid = new Codel[height, width];
        InitializeGrid();
    }

    public void FillWithRandomValues()
    {
        for (int yPosition = 0; yPosition < Height; yPosition++)
        {
            for (int xPosition = 0; xPosition < Width; xPosition++)
            {
                _codelGrid[yPosition, xPosition].Color =
                    PietColorFactory.CreateRandomColor();
            }
        }
    }
    public void SetCodelColor(int xPosition, int yPosition, PietColor color)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width-1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height-1);

        _codelGrid[yPosition, xPosition].Color = color;
    }

    public Codel GetCodel(int xPosition, int yPosition)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width-1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height-1);

        return _codelGrid[yPosition, xPosition];
    }
}
