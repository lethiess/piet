using System.Collections.Immutable;
using Piet.Grid;

namespace Piet.Interpreter;

public class CodelBlockSearcher : ICodelBlockSearcher
{
    public ICodelGrid CodelGrid { get; set; }
    
    public CodelBlockSearcher(ICodelGrid codelGrid)
    {
        CodelGrid = codelGrid;
    }

    private bool NeighborHasValidCoordinates(int xPosition, int yPosition)
    {
        return Enumerable.Range(0, CodelGrid.Width)
                   .Contains(xPosition)
               && Enumerable.Range(0, CodelGrid.Height)
                   .Contains(yPosition);
    }
    
    private ImmutableList<Codel> GetValidNeighbors(Codel codel)
    {
        var neighborCodels = new List<Codel>();
        var neighborCoordinates = new List<Coordinates>()
        {
            new (codel.XPosition, codel.YPosition + 1), // top neighbor
            new (codel.XPosition, codel.YPosition - 1), // bottom neighbor
            new (codel.XPosition + 1, codel.YPosition), // right neighbor
            new (codel.XPosition - 1, codel.YPosition)  // left neighbor
        }.Where(coordinates => NeighborHasValidCoordinates(coordinates.X, coordinates.Y)).ToList();
        
        neighborCoordinates.ForEach(coordinates => neighborCodels.Add(CodelGrid.GetCodel(coordinates.X, coordinates.Y)));

        return neighborCodels.ToImmutableList();
    }

    private IEnumerable<Codel> RegionGrowing(Codel seedcodel)
    {
        var codelBock = new List<Codel>();
        var codelBlockCandidates = new Stack<Codel>();
        bool[,] visited = new bool[CodelGrid.Height, CodelGrid.Width];

        codelBlockCandidates.Push(seedcodel);

        while (codelBlockCandidates.Count > 0)
        {
            var currentCodel = codelBlockCandidates.Pop();
            visited[currentCodel.YPosition, currentCodel.XPosition] = true;

            if (currentCodel.Color == seedcodel.Color)
            {
                codelBock.Add(currentCodel);

                var newNeighbors = GetValidNeighbors(currentCodel)
                    .Where(codel => visited[codel.YPosition, codel.XPosition] is false && codelBlockCandidates.Contains(codel) is false).ToList();
                newNeighbors.ForEach(x => codelBlockCandidates.Push(x));
            }
        }

        return codelBock;
    }

    public IEnumerable<Codel> GetCodelBock(Codel currentCodel)
    {
        return RegionGrowing(currentCodel);
    }
}