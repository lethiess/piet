using Piet.Color;
using System.Linq;

namespace Piet.Grid;
public sealed class PietDataGrid
{
    private PietColor[,] _girdData;

    public int Height { get; init; }
    public int Width { get; init; }

    public PietDataGrid(int height, int width)
    {
        Height = height;
        Width = width;
        _girdData = new PietColor[height, width];
        FillWithRandomValues();
    }

    public void FillWithRandomValues()
    {
        Random random = new Random();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                _girdData[i, j] = new PietColor().GetRandomColor();
            }
        }
    }

    public void SetRandomCellColor(int i, int j)
    {
        Random random = new Random();
        _girdData[i, j] = new PietColor().GetRandomColor();
    }

    public PietColor GetCell(int i, int j)
    {
        return _girdData[i, j];
    }

}
