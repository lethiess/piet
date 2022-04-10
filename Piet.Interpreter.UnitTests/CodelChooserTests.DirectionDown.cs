using Piet.Color;
using Piet.Grid;
using Xunit;

namespace Piet.Interpreter.UnitTests;

public partial class CodelChooserTests
{
    [Fact]
    public void GetNextCodel_DirectionPointerDown_CodelChooserLeft_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        // W W W W W W W
        // W W W W W W W
        // W W R R R W W
        // W W R R R W W
        // W W R R R W W
        // W W W W G W W
        // W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(4, 5, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Down;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerDown_CodelChooserRight_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        // W W W W W W W
        // W W W W W W W
        // W W R R R W W
        // W W R R R W W
        // W W R R R W W
        // W W G W W W W
        // W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(2, 5, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Down;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerDown_CodelChooserLeft_TraverseWhiteCodels_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        // W W W W W W W
        // W W W W W W W
        // W W R R R W W
        // W W R R R W W
        // W W R R R W W
        // W W W W W W W
        // W W W W G W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(4, 6, PietColors.Green);

        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Down;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerDown_CodelChooserRight_TraverseWhiteCodels_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        // W W W W W W W
        // W W W W W W W
        // W W R R R W W
        // W W R R R W W
        // W W R R R W W
        // W W W W W W W
        // W W G W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(2, 6, PietColors.Green);

        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Down;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }
}