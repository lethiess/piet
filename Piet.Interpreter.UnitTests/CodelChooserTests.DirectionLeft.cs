using Piet.Color;
using Piet.Grid;
using Xunit;

namespace Piet.Interpreter.UnitTests;

public partial class CodelChooserTests
{
    [Fact]
    public void GetNextCodel_DirectionPointerLeft_CodelChooserLeft_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        //   | 0 1 2 3 4 5 6
        // --+--------------
        // 0 | W W W W W W W
        // 1 | W W W W W W W
        // 2 | W W R R R W W
        // 3 | W W R R R W W
        // 4 | W G R R R W W
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(1, 4, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Left;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.False(nextCodelResult.TraversedWhiteCodels);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerLeft_CodelChooserRight_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        //   | 0 1 2 3 4 5 6
        // --+--------------
        // 0 | W W W W W W W
        // 1 | W W W W W W W
        // 2 | W G R R R W W
        // 3 | W W R R R W W
        // 4 | W W R R R W W
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(1, 2, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Left;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.False(nextCodelResult.TraversedWhiteCodels);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerLeft_CodelChooserLeft_TraverseWhiteCodels_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        //   | 0 1 2 3 4 5 6
        // --+--------------
        // 0 | W W W W W W W
        // 1 | W W W W W W W
        // 2 | W W R R R W W
        // 3 | W W R R R W W
        // 4 | G W R R R W W
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(0, 4, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Left;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.True(nextCodelResult.TraversedWhiteCodels);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerLeft_CodelChooserRight_TraverseWhiteCodels_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        //   | 0 1 2 3 4 5 6
        // --+--------------
        // 0 | W W W W W W W
        // 1 | W W W W W W W
        // 2 | G W R R R W W
        // 3 | W W R R R W W
        // 4 | W W R R R W W
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(0, 2, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Left;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.True(nextCodelResult.TraversedWhiteCodels);
    }
}