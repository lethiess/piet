using System.ComponentModel;
using Piet.Color;
using Piet.Grid;

namespace Piet.Interpreter;

internal sealed class CodelChooser : ICodelChooser
{
    public ICodelGrid CodelGrid { get; init; }

    public CodelChooser(ICodelGrid codelGrid)
    {
        CodelGrid = codelGrid;
    }

    public Codel GetNextCodel(IEnumerable<Codel> currentCodelBock)
    {
        // todo: loop until valid next codel was found or terminate 

        // #1 get codel block egde e which is most furthest away in direction of the DP
        //    this can include more than one codels (edge can be disjoint)
        var codelEdge = GetCodelEgde(currentCodelBock.ToList());

        // #2 inspect edge e and find codel c in current codel block which
        //    - is furthest away to the CC's directions of the DP's direction of travel
        Codel transitionCodel = GetCodelForTransitionToNextCodelBlock(codelEdge);
        
        // #3 travel from codel c to next codel in direction of DP
        //    - next codel is valid -> terminate loop and return
        //    - next codel is invalid -> toggle CodelChooser state
        //          - if still invalid move DP clockwise and try again
        var nextCodelCandidate = GetNextCodelCandidate(transitionCodel);

        throw new NotImplementedException();
    }

    private bool CodelCoordinatesAreValid(Coordinates codelCoordinates) =>
        PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up => codelCoordinates.Y >= 0,
            PietInterpreter.Direction.Right => codelCoordinates.X + 1 < CodelGrid.Width,
            PietInterpreter.Direction.Down => codelCoordinates.Y < CodelGrid.Height,
            PietInterpreter.Direction.Left => codelCoordinates.X >= 0,
            _ => throw new InvalidEnumArgumentException(
                $"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    private Coordinates GetCoordinatesForNextCodelInDirectionOfDirectionPointer(Codel currentCodel) =>
        PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up => new Coordinates(currentCodel.XPosition, currentCodel.YPosition - 1),
            PietInterpreter.Direction.Right => new Coordinates(currentCodel.XPosition + 1, currentCodel.YPosition),
            PietInterpreter.Direction.Down => new Coordinates(currentCodel.XPosition, currentCodel.YPosition + 1),
            PietInterpreter.Direction.Left => new Coordinates(currentCodel.XPosition - 1, currentCodel.YPosition),
            _ => throw new InvalidEnumArgumentException(
                $"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    private Codel? GetNextCodelCandidate(Codel transitionCodelCandidate)
    {
        var nextCodelCoordinates =
            GetCoordinatesForNextCodelInDirectionOfDirectionPointer(transitionCodelCandidate);

        if (CodelCoordinatesAreValid(nextCodelCoordinates))
        {
            var nextCodel =
                CodelGrid.GetCodel(nextCodelCoordinates.X, nextCodelCoordinates.Y);

            if (nextCodel.Color == PietColors.Black)
            {
                return null;
            }

            if (nextCodel.Color == PietColors.White)
            {
                // travel through white codels
                return GetNextCodelCandidate(nextCodel);
            }

            return nextCodel;
        }

        return null;
    }
    
    // Choose codel as possible candidate for according to:
    // https://dangermouse.net/esoteric/piet.html section 'Program Execution'
    //  ------------------------------
    //  DP      CC      Codel choosen 
    //  ------------------------------
    //  up      left    leftmost
    //          right   rightmost
    //  ------------------------------
    //  right   left    uppermost
    //          right   lowermost
    //  ------------------------------
    //  down    left    rightmost
    //          right   leftmost
    //  ------------------------------
    //  left    left    lowermost
    //          right   uppermost
    //  ------------------------------
    internal Codel GetCodelForTransitionToNextCodelBlock(List<Codel> codelEdge) =>
        PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Left => codelEdge
                    .OrderBy(codel => codel.XPosition)
                    .First(),
            PietInterpreter.Direction.Up when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Right => codelEdge
                    .OrderBy(codel => codel.XPosition)
                    .Last(),
            PietInterpreter.Direction.Right when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Left => codelEdge
                    .OrderBy(codel => codel.YPosition)
                    .First(),
            PietInterpreter.Direction.Right when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Right => codelEdge
                    .OrderBy(codel => codel.YPosition)
                    .Last(),
            PietInterpreter.Direction.Down when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Left => codelEdge
                    .OrderBy(codel => codel.XPosition)
                    .Last(),
            PietInterpreter.Direction.Down when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Right => codelEdge
                    .OrderBy(codel => codel.XPosition)
                    .First(),
            PietInterpreter.Direction.Left when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Left => codelEdge
                    .OrderBy(codel => codel.YPosition)
                    .Last(),
            PietInterpreter.Direction.Left when
                PietInterpreter.CodelChooserState is PietInterpreter.CodelChooser.Right => codelEdge
                    .OrderBy(codel => codel.YPosition)
                    .First(),
            _ => throw new InvalidEnumArgumentException($"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    internal int
        GetFurthestCodelInDirectionPointersDirections(IEnumerable<Codel> currentCodelBlock) =>
        PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up => currentCodelBlock.Min(codel => codel.YPosition),
            PietInterpreter.Direction.Right => currentCodelBlock.Max(codel => codel.XPosition),
            PietInterpreter.Direction.Down => currentCodelBlock.Max(codel => codel.YPosition),
            PietInterpreter.Direction.Left => currentCodelBlock.Min(codel => codel.XPosition),
            _ => throw new InvalidEnumArgumentException($"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    internal List<Codel> GetCodelEgde(List<Codel> currentCodelBlock)
    {
        var edgeValue = GetFurthestCodelInDirectionPointersDirections(currentCodelBlock);

        var codelBlockEdge = PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up or PietInterpreter.Direction.Down =>
                currentCodelBlock.Where(codel => codel.YPosition == edgeValue),
            PietInterpreter.Direction.Right or PietInterpreter.Direction.Left =>
                currentCodelBlock.Where(codel => codel.XPosition == edgeValue),
            _ => throw new InvalidEnumArgumentException($"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

        return codelBlockEdge.ToList();
    }
}