using Piet.Color;
using Piet.Grid;

namespace Piet.Interpreter;

public sealed class CodelChooser : ICodelChooser
{
    // 8 Retries 
    // 4 directions with 2 codel chooser states per direction
    // the program terminates if the max retries are reached
    // 
    private const int MAX_RETRY_COUNT = 8;
    public ICodelGrid CodelGrid { get; init; }

    public CodelChooser(ICodelGrid codelGrid)
    {
        CodelGrid = codelGrid;
    }

    public CodelResult GetNextCodel(IEnumerable<Codel> currentCodelBlock)
    {
        bool codelChooserWasToggled = false;
        for (int i = 0; i < MAX_RETRY_COUNT; i++)
        {
            // #1 get codel block egde e which is most furthest away in direction of the DP
            //    this can include more than one codels (edge can be disjoint)
            var codelEdge = GetCodelEgde(currentCodelBlock.ToList());

            // #2 inspect edge e and find codel c in current codel block which
            //    - is furthest away to the CC's directions of the DP's direction of travel
            Codel transitionCodel = GetCodelForTransitionToNextCodelBlock(codelEdge);
            
            // #3 travel from codel c to next codel in direction of DP
            //    - next codel is valid -> terminate loop and return
            //    - next codel is invalid -> toggle CodelChooser state
            //          - if still GetCodelBlockCandidate move DP clockwise and try again
            var nextCodelCandidate = GetCodelBlockCandidate(transitionCodel);


            if (nextCodelCandidate.Codel is not null)
            {
                nextCodelCandidate.Success = true;
                return nextCodelCandidate;
            }
            
            if (codelChooserWasToggled is false)
            {
                PietInterpreter.ToggleCodelChooser();
                codelChooserWasToggled = true;

            }
            else if (codelChooserWasToggled)
            {
                PietInterpreter.RotateDirectionPointerClockwise();
                codelChooserWasToggled = false;
            }
        }

        return new CodelResult() {Success = false};
    }

    private bool CodelCoordinatesAreValid(Coordinates codelCoordinates) =>
        PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up => codelCoordinates.Y >= 0,
            PietInterpreter.Direction.Right => codelCoordinates.X < CodelGrid.Width,
            PietInterpreter.Direction.Down => codelCoordinates.Y < CodelGrid.Height,
            PietInterpreter.Direction.Left => codelCoordinates.X >= 0,
            _ => throw new ArgumentOutOfRangeException(
                $"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    private static Coordinates GetCoordinatesForNextCodelInDirectionOfDirectionPointer(Codel currentCodel) =>
        PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up => new Coordinates(currentCodel.XPosition, currentCodel.YPosition - 1),
            PietInterpreter.Direction.Right => new Coordinates(currentCodel.XPosition + 1, currentCodel.YPosition),
            PietInterpreter.Direction.Down => new Coordinates(currentCodel.XPosition, currentCodel.YPosition + 1),
            PietInterpreter.Direction.Left => new Coordinates(currentCodel.XPosition - 1, currentCodel.YPosition),
            _ => throw new ArgumentOutOfRangeException(
                $"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    private CodelResult GetCodelBlockCandidate(Codel transitionCodelCandidate)
    {
        CodelResult result = new();
        var codelCandidate = GetNextCodelCandidate(transitionCodelCandidate);

        while (codelCandidate is not null)
        {
            if (codelCandidate.Color != PietColors.Black)
            {
                if (codelCandidate.Color == PietColors.White)
                {
                    result.TraversedWhiteCodels = true;
                    codelCandidate = GetNextCodelCandidate(codelCandidate);
                }
                else
                {
                    result.Codel = codelCandidate;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return result;
    }

    private Codel? GetNextCodelCandidate(Codel transitionCodelCandidate)
    {
        var nextCodelCoordinates =
            GetCoordinatesForNextCodelInDirectionOfDirectionPointer(transitionCodelCandidate);

        if (CodelCoordinatesAreValid(nextCodelCoordinates))
        {
            return CodelGrid.GetCodel(nextCodelCoordinates.X, nextCodelCoordinates.Y);
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
    internal static Codel GetCodelForTransitionToNextCodelBlock(List<Codel> codelEdge) =>
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
            _ => throw new ArgumentOutOfRangeException($"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    private static int
        GetFurthestCodelInDirectionPointersDirections(IEnumerable<Codel> currentCodelBlock) =>
        PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up => currentCodelBlock.Min(codel => codel.YPosition),
            PietInterpreter.Direction.Right => currentCodelBlock.Max(codel => codel.XPosition),
            PietInterpreter.Direction.Down => currentCodelBlock.Max(codel => codel.YPosition),
            PietInterpreter.Direction.Left => currentCodelBlock.Min(codel => codel.XPosition),
            _ => throw new ArgumentOutOfRangeException($"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

    private static List<Codel> GetCodelEgde(List<Codel> currentCodelBlock)
    {
        var edgeValue = GetFurthestCodelInDirectionPointersDirections(currentCodelBlock);

        var codelBlockEdge = PietInterpreter.DirectionPointer switch
        {
            PietInterpreter.Direction.Up or PietInterpreter.Direction.Down =>
                currentCodelBlock.Where(codel => codel.YPosition == edgeValue),
            PietInterpreter.Direction.Right or PietInterpreter.Direction.Left =>
                currentCodelBlock.Where(codel => codel.XPosition == edgeValue),
            _ => throw new ArgumentOutOfRangeException($"The value {PietInterpreter.DirectionPointer} of type {typeof(PietInterpreter.Direction)} is invalid in this context")
        };

        return codelBlockEdge.ToList();
    }
}