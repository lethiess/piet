using Piet.Color;
using Piet.Grid;
using Xunit;

namespace Piet.Interpreter.UnitTests;

public partial class CodelChooserTests
{

    [Fact]
    public void GetNextCodel_DirectionPointerRight_CodelChooserLeft_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        //   | 0 1 2 3 4 5 6
        // --+--------------
        // 0 | W W W W W W W
        // 1 | W W W W W W W
        // 2 | W W R R R G W
        // 3 | W W R R R W W
        // 4 | W W R R R W W
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(5, 2, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser{ CodelGrid = codelGrid };

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.False(nextCodelResult.TraversedWhiteCodels);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerRight_CodelChooserRight_MustMatch()
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
        // 4 | W W R R R G W
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(5, 4, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser{ CodelGrid = codelGrid };

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.False(nextCodelResult.TraversedWhiteCodels);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerRight_CodelChooserLeft_TraverseWhiteCodels_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        //   | 0 1 2 3 4 5 6
        // --+--------------
        // 0 | W W W W W W W
        // 1 | W W W W W W W
        // 2 | W W R R R W G
        // 3 | W W R R R W W
        // 4 | W W R R R W W
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(6, 2, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser{ CodelGrid = codelGrid };

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.True(nextCodelResult.TraversedWhiteCodels);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerRight_CodelChooserRight_TraverseWhiteCodels_MustMatch()
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
        // 4 | W W R R R W G
        // 5 | W W W W W W W
        // 6 | W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(6, 4, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser{ CodelGrid = codelGrid };

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodelResult = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodelResult);
        Assert.Equal(expectedNextCodel, nextCodelResult.Codel);
        Assert.True(nextCodelResult.TraversedWhiteCodels);
    }
}