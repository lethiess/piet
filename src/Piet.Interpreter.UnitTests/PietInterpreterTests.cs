using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Piet.Color;
using Piet.Grid;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;
using Xunit;

namespace Piet.Interpreter.UnitTests;

public class PietInterpreterTests
{
    [Fact]
    public void Run_EmptyCodelGrid_MustReturn_Success()
    {
        var codelGrid = new CodelGrid(10, 10, PietColors.White);
        var codelChooser = new CodelChooser{ CodelGrid = codelGrid };
        var codelBlockSearcher = new CodelBlockSearcher { CodelGrid = codelGrid };
        var inputFaceMock = new Mock<IInputFacade>();
        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            new OutputService(), inputFaceMock.Object);
        
        var interpreter = new PietInterpreter(new NullLogger<PietInterpreter>(),
            codelChooser,
            codelBlockSearcher,
            programOperator);

        var result = interpreter.Run(codelGrid);

        Assert.NotNull(result);
        Assert.Equal(PietInterpreterResult.InterpreterStatus.Success, result.Status);
        Assert.Equal("Successfully interpreted codel grid", result.Message);
    }

    [Fact]
    public void Run_AdditionExample_MustReturn_Success()
    {
        var codelGrid = new CodelGrid(10, 10, PietColors.White);
        // set first codel
        codelGrid.SetCodel(new Codel(0,0, PietColors.LightRed));
        // push 1 on the stack
        codelGrid.SetCodel(new Codel(1,0, PietColors.Red));
        codelGrid.SetCodel(new Codel(1,1, PietColors.Red)); 
        // push 2 on the stack
        codelGrid.SetCodel(new Codel(2,0, PietColors.DarkRed));
        // perform add operation
        codelGrid.SetCodel(new Codel(3,0, PietColors.DarkYellow));
        // none operation (white codel in between)
        codelGrid.SetCodel(new Codel(4,1, PietColors.Green));
        codelGrid.SetCodel(new Codel(5,0, PietColors.Green));
        codelGrid.SetCodel(new Codel(5,1, PietColors.Green));
        // blocker codels to force program exit
        codelGrid.SetCodel(new Codel(3,1, PietColors.Black));
        codelGrid.SetCodel(new Codel(4,2, PietColors.Black));
        codelGrid.SetCodel(new Codel(5,2, PietColors.Black));
        codelGrid.SetCodel(new Codel(6,1, PietColors.Black));
        codelGrid.SetCodel(new Codel(6,1, PietColors.Black));
        
        var codelChooser = new CodelChooser{ CodelGrid = codelGrid };
        var codelBlockSearcher = new CodelBlockSearcher { CodelGrid = codelGrid };
        var inputFacadeMock = new Mock<IInputFacade>();
        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            new OutputService(), inputFacadeMock.Object);

        var interpreter = new PietInterpreter(new NullLogger<PietInterpreter>(),
            codelChooser,
            codelBlockSearcher,
            programOperator);

        var result = interpreter.Run(codelGrid);

        Assert.NotNull(result);
        Assert.Equal(PietInterpreterResult.InterpreterStatus.Success, result.Status);
        Assert.Equal("Successfully interpreted codel grid", result.Message);
    }
}