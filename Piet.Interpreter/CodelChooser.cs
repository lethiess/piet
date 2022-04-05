using System.ComponentModel;
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
        // todo: determine next codel

        // loop until valid next codel was found or terminate 

        // #1 get codel block egde e which is most furthest away in direction of the DP
        //    this can include more than one codels (edge can be disjoint)
        var codelEdge = GetCodelEgde(currentCodelBock.ToList());

        // #2 inspect edge e and find codel c in current codel block which
        //    - is furthest away to the CC's directions of the DP's direction of travel
        var transitionCodelCandidate = GetCodelForTransitionToNextCodelBlock(codelEdge);
        
        // #3 travel from codel c to next codel in direction of DP
        //    - next codel is valid -> terminate loop and return
        //    - next codel is invalid -> toggle CodelChooser state
        //          - if still invalid move DP clockwise and try again
        

        // restriction of next blocks:
        // - black block is invalid
        // - out of bound is invalid
        // - white blocks can be traversed (for the next block the previous rules apply)
        //      - change the color without triggering an command


        throw new NotImplementedException();
    }

    // Choose codel as possible candidate for  according to:
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