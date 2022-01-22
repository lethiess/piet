using System.Drawing;
using System.Linq;

namespace Piet.Grid;
public sealed class PietDataGrid
{
    private Color[,] _girdData;

    public int Height { get; init; }
    public int Width { get; init; }

    public PietDataGrid(int height, int width)
    {
        Height = height;
        Width = width;
        _girdData = new Color[height, width];
        FillWithRandomValues();
    }

    public void FillWithRandomValues()
    {
        Random random = new Random();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                _girdData[i, j] = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            }
        }
    }

    public void SetRandomCellColor(int i, int j)
    {
        Random random = new Random();
        _girdData[i, j] = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
    }

    public Color GetCell(int i, int j)
    {
        return _girdData[i, j];
    }

}
