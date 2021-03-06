using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Moq;
using Piet.Color;
using Piet.Command;
using Piet.Interpreter.Exceptions;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter.UnitTests;

public class ProgramOperatorTests
{
    [Fact]
    public void None_EmptyStack_MustMatchInitialStack()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        var initialProgramStack = programOperator.GetProgramStack();

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.None), 0, null);

        var currentProgramStack = programOperator.GetProgramStack();

        Assert.Equal(initialProgramStack, currentProgramStack);
    }

    [Fact]
    public void None_StackIsNotEmpty_MustMatchProgramStackBeforeOperation()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 1, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 2, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 3, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 4, null);

        var expectedProgramStackAsArray = programOperator.GetProgramStack();
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.None), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();
        Assert.Equal(expectedProgramStackAsArray, currentProgramStackAsArray);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(1337)]
    [InlineData(31456233)]
    public void Push_InitialStackIsEmpty_MustMatch(int codelBlockSize)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), codelBlockSize, null);

        var expectedProgramStackAsArray = programOperator.GetProgramStack();
        Assert.Single(expectedProgramStackAsArray);
        Assert.Equal(codelBlockSize, expectedProgramStackAsArray[0]);
    }

    [Fact]
    public void Push_InitialStackIsEmpty_AddMultipleValues_MustMatchStackSize()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        const int stackSize = 100;
        for (int i = 0; i < stackSize; i++)
        {
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), i, null);
        }

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Equal(stackSize, currentProgramStackAsArray.Count);
    }

    [Fact]
    public void Pop_StackIsEmpty_MustNotThrow_MustMatchInitialStack()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        var initialProgramStack = programOperator.GetProgramStack();

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pop), 0, null);

        var currentProgramStack = programOperator.GetProgramStack();

        Assert.Equal(initialProgramStack, currentProgramStack);
    }

    [Fact]
    public void Pop_StackHasSufficientValues_MustMatch()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 1, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 2, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 3, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 4, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pop), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Equal(3, currentProgramStackAsArray.Count);
        Assert.Equal(3, currentProgramStackAsArray[0]);
        Assert.Equal(2, currentProgramStackAsArray[1]);
        Assert.Equal(1, currentProgramStackAsArray[2]);
    }

    [Fact]
    public void Add_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Add),
                0, null));
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(31, -21, 10)]
    [InlineData(0, 0, 0)]
    [InlineData(-12, -12, -24)]
    public void Add_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandA, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandB, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Add), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Subtract_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Subtract),
                0, null));
    }

    [Theory]
    [InlineData(5, 2, 3)]
    [InlineData(1, 1, 0)]
    [InlineData(31, -21, 52)]
    [InlineData(0, 0, 0)]
    [InlineData(-12, -12, 0)]
    public void Subtract_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandA, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandB, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Subtract), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Multiply_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Multiply),
                0, null));
    }

    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(4, 4, 16)]
    [InlineData(0, 0, 0)]
    [InlineData(3, -7, -21)]
    [InlineData(-9, -5, 45)]
    public void Multiply_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandA, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandB, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Multiply), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Divide_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Divide),
                0, null));
    }

    [Fact]
    public void Divide_DivisionByZero_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 12, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 0, null);

        Assert.Throws<PietInterpreterDividedByZeroException>(() =>
            programOperator.ExecuteCommand(
                new ColorCommand(PietColors.Blue, Command.Command.Divide), 0, null));
    }

    [Theory]
    [InlineData(21, 3, 7)]
    [InlineData(4, 4, 1)]
    [InlineData(0, 1, 0)]
    [InlineData(-21, 3, -7)]
    [InlineData(-21, -3, 7)]
    public void Divide_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandA, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandB, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Divide), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Modulo_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Modulo),
                0, null));
    }

    [Fact]
    public void Modulo_DivisionByZero_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 12, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 0, null);

        Assert.Throws<PietInterpreterDividedByZeroException>(() =>
            programOperator.ExecuteCommand(
                new ColorCommand(PietColors.Blue, Command.Command.Modulo), 0, null));
    }

    [Theory]
    [InlineData(21, 3, 0)]
    [InlineData(7, 4, 3)]
    [InlineData(0, 1, 0)]
    [InlineData(-21, 2, 1)]
    [InlineData(-21, -2, 1)]
    public void Modulo_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandA, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandB, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Modulo), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Not_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Not),
                0, null));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 0)]
    [InlineData(1, 0)]
    public void Not_StackHasSufficientValues_MustMatch(int operand, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operand, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Not), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void GreaterThan_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Greater),
                0, null));
    }

    [Theory]
    [InlineData(21, 3, 1)]
    [InlineData(1337, 42, 1)]
    [InlineData(1, 1, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(-1, 1, 0)]
    public void GreaterThan_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandA, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), operandB, null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Greater), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Pointer_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pointer),
                0, null));
    }

    [Theory]
    [InlineData(1, PietInterpreter.Direction.Left, PietInterpreter.Direction.Up)]
    [InlineData(2, PietInterpreter.Direction.Left, PietInterpreter.Direction.Right)]
    [InlineData(3, PietInterpreter.Direction.Left, PietInterpreter.Direction.Down)]
    [InlineData(4, PietInterpreter.Direction.Left, PietInterpreter.Direction.Left)]
    [InlineData(12334, PietInterpreter.Direction.Left, PietInterpreter.Direction.Right)]
    [InlineData(-1, PietInterpreter.Direction.Left, PietInterpreter.Direction.Down)]
    [InlineData(-2, PietInterpreter.Direction.Left, PietInterpreter.Direction.Right)]
    [InlineData(-3, PietInterpreter.Direction.Left, PietInterpreter.Direction.Up)]
    [InlineData(-4, PietInterpreter.Direction.Left, PietInterpreter.Direction.Left)]
    [InlineData(-12334, PietInterpreter.Direction.Left, PietInterpreter.Direction.Right)]
    public void Pointer_StackHasSufficientValues_MustMatch(int numberOfPointerRotations, PietInterpreter.Direction initialDirection, PietInterpreter.Direction expectedDirection)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        PietInterpreter.DirectionPointer = initialDirection;

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), numberOfPointerRotations, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pointer), 0, null);

        Assert.Equal(expectedDirection, PietInterpreter.DirectionPointer);
    }

    [Fact]
    public void Switch_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Switch),
                0, null));
    }

    [Theory]
    [InlineData(1, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Right)]
    [InlineData(2, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Left)]
    [InlineData(3, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Right)]
    [InlineData(0, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Left)]
    [InlineData(-1, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Right)]
    [InlineData(-2, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Left)]
    [InlineData(-3, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Right)]

    public void Switch_StackHasSufficientValues_MustMatch(int numberOfSwitches, PietInterpreter.CodelChooser initialCodelChooserState, PietInterpreter.CodelChooser expectedCodelChooserState)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        PietInterpreter.CodelChooserState = initialCodelChooserState;

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), numberOfSwitches, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Switch), 0, null);

        Assert.Equal(expectedCodelChooserState, PietInterpreter.CodelChooserState);
    }

    [Fact]
    public void Duplicate_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Duplicate),
                0, null));
    }

    [Theory]
    [InlineData(1, 2, 1)]
    [InlineData(0, 2, 0)]
    [InlineData(-11, 2, -11)]
    public void Duplicate_StackHasSufficientValues_MustMatch(int stackValue, int expectedStackSize, int expectedStackValueForAllElements)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), stackValue, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Duplicate), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Equal(expectedStackSize, currentProgramStackAsArray.Count);
        Assert.All(currentProgramStackAsArray,
            i => Assert.Equal(expectedStackValueForAllElements, i));
    }

    [Fact]
    public void Roll_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll),
                0, null));
    }

    [Fact]
    public void Roll_InsufficientNumberOfElementsForRollOperation_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);


        int depthOfRollOperation = 3;
        int numberOfRolls = 1;
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 0, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 0, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), depthOfRollOperation, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), numberOfRolls, null);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll),
                0, null));
    }

    [Fact]
    public void Roll_DepthOfRollsExceedsTheStackSize_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);


        int depthOfRollOperation = -1;
        int numberOfRolls = 1;
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 0, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 0, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), depthOfRollOperation, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), numberOfRolls, null);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll),
                0, null));
    }

    [Fact]
    public void Roll_DepthOfRollIsThree_OneRoll_MustMatch()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        int depthOfRollOperation = 3;
        int numberOfRolls = 1;

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 1, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 2, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 3, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 4, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 5, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), depthOfRollOperation, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), numberOfRolls, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Equal(5, currentProgramStackAsArray.Count);
        Assert.Equal(4, currentProgramStackAsArray[0]);
        Assert.Equal(3, currentProgramStackAsArray[1]);
        Assert.Equal(2, currentProgramStackAsArray[2]);
        Assert.Equal(5, currentProgramStackAsArray[3]);
        Assert.Equal(1, currentProgramStackAsArray[4]);
    }

    [Fact]
    public void Roll_DepthOfRollIsThree_TwoRolls_MustMatch()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        int depthOfRollOperation = 3;
        int numberOfRolls = 2;

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 1, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 2, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 3, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 4, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 5, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), depthOfRollOperation, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), numberOfRolls, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Equal(5, currentProgramStackAsArray.Count);
        Assert.Equal(3, currentProgramStackAsArray[0]);
        Assert.Equal(2, currentProgramStackAsArray[1]);
        Assert.Equal(5, currentProgramStackAsArray[2]);
        Assert.Equal(4, currentProgramStackAsArray[3]);
        Assert.Equal(1, currentProgramStackAsArray[4]);
    }

    [Fact]
    public void InputNumber_StackHasSufficientValues_MustRequestInput()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();
        inputServiceMock.Setup(x => x.RequestIntegerInputAsync());

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputNumber), 0, new Context() { Pause = null});

        inputServiceMock.Verify(service => service.RequestIntegerInputAsync(), Times.Once);
    }

    [Fact]
    public void InputNumber_StackHasSufficientValues_MustRequestInput_MustCallPause()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();
        inputServiceMock.Setup(x => x.RequestIntegerInputAsync());

        var pauseActionMock = new Mock<Action>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputNumber), 0, new Context() { Pause = pauseActionMock.Object });

        inputServiceMock.Verify(service => service.RequestIntegerInputAsync(), Times.Once);
        pauseActionMock.Verify(action => action.Invoke(), Times.Once);
    }

    [Fact]
    public void InputCharacter_StackHasSufficientValues_MustRequestInput()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();
        inputServiceMock.Setup(x => x.RequestCharacterInputAsync());

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputCharacter), 0, new Context() { Pause = null });

        inputServiceMock.Verify(service => service.RequestCharacterInputAsync(), Times.Once);
    }

    [Fact]
    public void InputCharacter_StackHasSufficientValues_MustRequestInput_MustCallPause()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();
        inputServiceMock.Setup(x => x.RequestCharacterInputAsync());

        var pauseActionMock = new Mock<Action>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputCharacter), 0, new Context()
            {
                Pause = pauseActionMock.Object
            });

        inputServiceMock.Verify(service => service.RequestCharacterInputAsync(), Times.Once);
        pauseActionMock.Verify(action => action.Invoke(), Times.Once);
    }


    [Fact]
    public void OutputNumber_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.OutputNumber),
                0, null));
    }

    [Fact]
    public void OutputNumber_StackHasSufficientValues_MustMatch()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 42, null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.OutputNumber), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Empty(currentProgramStackAsArray);
    }

    [Fact]
    public void OutputCharacter_StackIsEmpty_MustThrow()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        Assert.Throws<InsufficientNumberOfElementsOnProgramStackException>(() =>
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.OutputCharacter),
                0, null));
    }

    [Fact]
    public void OutputCharacter_StackHasSufficientValues_MustMatch()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), 'c', null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.OutputCharacter), 0, null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Empty(currentProgramStackAsArray);
    }

}