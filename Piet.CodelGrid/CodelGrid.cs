using Dawn;
using Piet.Color;

namespace Piet.Grid;

public sealed class CodelGrid
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

    public void SetRandomCodelColor(int yPosition, int xPosition)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width-1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height-1);

        _codelGrid[yPosition, xPosition].Color = PietColorFactory.CreateRandomColor();
    }

    public void SetUniqueCodelColor(PietColor color)
    {
        for (int yPosition = 0; yPosition < Height; yPosition++)
        {
            for (int xPosition = 0; xPosition < Width; xPosition++)
            {
                _codelGrid[yPosition, xPosition].Color = color;
            }
        }
    }

    public void SetCodelColor(int yPosition, int xPosition, PietColor color)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width-1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height-1);

        _codelGrid[yPosition, xPosition].Color = color;
    }

    public Codel GetCodel(int yPosition, int xPosition)
    {
        Guard.Argument(xPosition, nameof(xPosition))
            .InRange(0, Width-1);
        Guard.Argument(yPosition, nameof(yPosition))
            .InRange(0, Height-1);

        return _codelGrid[yPosition, xPosition];
    }
}
