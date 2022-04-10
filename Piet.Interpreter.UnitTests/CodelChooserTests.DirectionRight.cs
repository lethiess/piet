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
        // W W W W W W W
        // W W W W W W W
        // W W R R R G W
        // W W R R R W W
        // W W R R R W W
        // W W W W W W W
        // W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(5, 2, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerRight_CodelChooserRight_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        // W W W W W W W
        // W W W W W W W
        // W W R R R W W
        // W W R R R W W
        // W W R R R G W
        // W W W W W W W
        // W W W W W W W

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(5, 4, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerRight_CodelChooserLeft_TraverseWhiteCodels_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        // W W W W W W W 
        // W W W W W W W 
        // W W R R R W G 
        // W W R R R W W 
        // W W R R R W W 
        // W W W W W W W 
        // W W W W W W W 

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(6, 2, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }

    [Fact]
    public void GetNextCodel_DirectionPointerRight_CodelChooserRight_TraverseWhiteCodels_MustMatch()
    {
        // codel grid under test
        // values: (W := white, R := red (current codel block), G := green (expected next codel)
        // 
        // W W W W W W W 
        // W W W W W W W 
        // W W R R R W W 
        // W W R R R W W 
        // W W R R R W G 
        // W W W W W W W 
        // W W W W W W W 

        var codelGrid = GetInitialCodelGrid();
        var expectedNextCodel = new Codel(6, 4, PietColors.Green);
        codelGrid.SetCodel(expectedNextCodel);

        var codelChooser = new CodelChooser(codelGrid);

        PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
        PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Right;

        var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

        Assert.NotNull(nextCodel);
        Assert.Equal(expectedNextCodel, nextCodel);
    }
}