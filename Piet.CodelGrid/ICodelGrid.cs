using Piet.Color;

namespace Piet.Grid;

public interface ICodelGrid
{
    public int Width { get; }
    public int Height { get; }
    public Codel GetCodel(int xPosition, int yPosition);
    public void SetCodelColor(int xPosition, int yPosition, PietColor color); 
}