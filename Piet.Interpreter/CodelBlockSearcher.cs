using System.Collections.Immutable;
using Piet.Grid;

namespace Piet.Interpreter;

internal class CodelBlockSearcher : ICodelBlockSearcher
{
    private ICodelGrid _grid;

    public CodelBlockSearcher(ICodelGrid grid)
    {
        _grid = grid;
    }

    public void Initialize(ICodelGrid codelGrid) => _grid = codelGrid;


    private bool NeightborHasValidCoordinates(int xPosition, int yPosition)
    {
        return Enumerable.Range(0, _grid.Width)
                   .Contains(xPosition)
               && Enumerable.Range(0, _grid.Height)
                   .Contains(yPosition);
    }

    private sealed record Coordinates(int X, int Y) { }

    private ImmutableList<Codel> GetValidNeighbors(Codel codel)
    {
        var neighborCodels = new List<Codel>();
        var neighborCoordinates = new List<Coordinates>()
        {
            new (codel.XPosition, codel.YPosition + 1), // top neighbor
            new (codel.XPosition, codel.YPosition - 1), // bottom neighbor
            new (codel.XPosition + 1, codel.YPosition), // right neightbor
            new (codel.XPosition - 1, codel.YPosition)  // left neighbor
        }.Where(coordinates => NeightborHasValidCoordinates(coordinates.Y, coordinates.Y)).ToList();
        
        neighborCoordinates.ForEach(coordinates => neighborCodels.Add(_grid.GetCodel(coordinates.X, coordinates.Y)));

        return neighborCodels.ToImmutableList();
    }

    private IEnumerable<Codel> RegionGrowing(Codel seedcodel)
    {
        var codelBock = new List<Codel>();
        var codelBlockCandidates = new Stack<Codel>();
        codelBlockCandidates.Push(seedcodel);

        while (codelBlockCandidates.Count > 0)
        {
            var currentCodel = codelBlockCandidates.Pop();
            if (currentCodel.Color == seedcodel.Color)
            {
                codelBock.Add(currentCodel);
            }

            var newNeighbors = GetValidNeighbors(currentCodel)
                .Where(x => codelBock.Contains(x) is false).ToList();
            newNeighbors.ForEach(x => codelBlockCandidates.Push(x));
        }

        return codelBock;
    }

    public IEnumerable<Codel> GetCodelBock(Codel currentCodel)
    {
        return RegionGrowing(currentCodel);
    }
}