using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FizzWare.NBuilder;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Moq;
using Piet.Color;
using Piet.Command;
using Piet.Grid;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter.UnitTests;

public class ProgramOperatorTests
{
    private bool _onErrorActionWasTriggered = false;
    private void TestOnError()
    {
        _onErrorActionWasTriggered = true;
    }

    private static ImmutableList<Codel> GetFakeCodelBlockOfSize(int size)
    {
        if (size == 0)
        {
            return new List<Codel>().ToImmutableList();
        }

        return Builder<Codel>.CreateListOfSize(size)
            .All()
            .WithFactory(() => new Codel(0, 0, PietColors.Black))
            .Build()
            .ToImmutableList();
    }


    [Fact]
    public void None_EmptyStack_MustMatchInitialStack()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        var initialProgramStack = programOperator.GetProgramStack();

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.None), new List<Codel>().ToImmutableList(), null);

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(1), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(2), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(3), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(4), null);

        var expectedProgramStackAsArray = programOperator.GetProgramStack();
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.None), new List<Codel>().ToImmutableList(), null);

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(codelBlockSize), null);

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
            programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(i), null);
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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pop), new List<Codel>().ToImmutableList(), null);

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(1), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(2), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(3), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(4), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pop), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Equal(3, currentProgramStackAsArray.Count);
        Assert.Equal(3, currentProgramStackAsArray[0]);
        Assert.Equal(2, currentProgramStackAsArray[1]);
        Assert.Equal(1, currentProgramStackAsArray[2]);
    }

    [Fact]
    public void Add_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Add),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(0, 0, 0)]
    public void Add_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandA), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandB), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Add), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Subtract_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Subtract),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(5, 2, 3)]
    [InlineData(1, 1, 0)]
    [InlineData(0, 0, 0)]
    public void Subtract_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandA), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandB), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Subtract), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Multiply_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Multiply),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(4, 4, 16)]
    [InlineData(0, 0, 0)]
    public void Multiply_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandA), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandB), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Multiply), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Divide_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Divide),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Fact]
    public void Divide_DivisionByZero_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;

        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(12), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), new List<Codel>().ToImmutableList(), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Divide),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(21, 3, 7)]
    [InlineData(4, 4, 1)]
    [InlineData(0, 1, 0)]
    public void Divide_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandA), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandB), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Divide), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Modulo_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Modulo),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });
        Assert.True(_onErrorActionWasTriggered);
    }

    [Fact]
    public void Modulo_DivisionByZero_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;

        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(12), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), new List<Codel>().ToImmutableList(), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Modulo),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(21, 3, 0)]
    [InlineData(7, 4, 3)]
    [InlineData(0, 1, 0)]
    public void Modulo_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandA), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandB), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Modulo), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Not_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Not),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void Not_StackHasSufficientValues_MustMatch(int operand, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operand), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Not), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void GreaterThan_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;

        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

       programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Greater),
                new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError});
        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(21, 3, 1)]
    [InlineData(1337, 42, 1)]
    [InlineData(1, 1, 0)]
    [InlineData(0, 1, 0)]
    public void GreaterThan_StackHasSufficientValues_MustMatch(int operandA, int operandB, int expectedResult)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandA), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(operandB), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Greater), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Single(currentProgramStackAsArray);
        Assert.Equal(expectedResult, currentProgramStackAsArray[0]);
    }

    [Fact]
    public void Pointer_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);


        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pointer),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });
        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(1, PietInterpreter.Direction.Left, PietInterpreter.Direction.Up)]
    [InlineData(2, PietInterpreter.Direction.Left, PietInterpreter.Direction.Right)]
    [InlineData(3, PietInterpreter.Direction.Left, PietInterpreter.Direction.Down)]
    [InlineData(4, PietInterpreter.Direction.Left, PietInterpreter.Direction.Left)]
    [InlineData(12334, PietInterpreter.Direction.Left, PietInterpreter.Direction.Right)]
    public void Pointer_StackHasSufficientValues_MustMatch(int numberOfPointerRotations, PietInterpreter.Direction initialDirection, PietInterpreter.Direction expectedDirection)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        PietInterpreter.DirectionPointer = initialDirection;

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(numberOfPointerRotations), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Pointer), new List<Codel>().ToImmutableList(), null);

        Assert.Equal(expectedDirection, PietInterpreter.DirectionPointer);
    }

    [Fact]
    public void Switch_StackIsEmpty_MThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Switch),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(1, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Right)]
    [InlineData(2, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Left)]
    [InlineData(3, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Right)]
    [InlineData(0, PietInterpreter.CodelChooser.Left, PietInterpreter.CodelChooser.Left)]

    public void Switch_StackHasSufficientValues_MustMatch(int numberOfSwitches, PietInterpreter.CodelChooser initialCodelChooserState, PietInterpreter.CodelChooser expectedCodelChooserState)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        PietInterpreter.CodelChooserState = initialCodelChooserState;

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(numberOfSwitches), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Switch), new List<Codel>().ToImmutableList(), null);

        Assert.Equal(expectedCodelChooserState, PietInterpreter.CodelChooserState);
    }

    [Fact]
    public void Duplicate_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;

        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Duplicate),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Theory]
    [InlineData(1, 2, 1)]
    [InlineData(0, 2, 0)]
    public void Duplicate_StackHasSufficientValues_MustMatch(int stackValue, int expectedStackSize, int expectedStackValueForAllElements)
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(stackValue), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Duplicate), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Equal(expectedStackSize, currentProgramStackAsArray.Count);
        Assert.All(currentProgramStackAsArray,
            i => Assert.Equal(expectedStackValueForAllElements, i));
    }

    [Fact]
    public void Roll_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        

        Assert.True(_onErrorActionWasTriggered);
    }

    [Fact]
    public void Roll_InsufficientNumberOfElementsForRollOperation_ThrowsInternalException_MustInvoke_OnErrorAction
        ()
    {
        _onErrorActionWasTriggered = false;

        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);


        int depthOfRollOperation = 3;
        int numberOfRolls = 1;
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), new List<Codel>().ToImmutableList(), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), new List<Codel>().ToImmutableList(), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(depthOfRollOperation), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(numberOfRolls), null);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });
        Assert.True(_onErrorActionWasTriggered);
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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(1), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(2), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(3), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(4), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(5), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(depthOfRollOperation), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(numberOfRolls), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll), new List<Codel>().ToImmutableList(), null);

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(1), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(2), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(3), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(4), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(5), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(depthOfRollOperation), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(numberOfRolls), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.Roll), new List<Codel>().ToImmutableList(), null);

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputNumber), new List<Codel>().ToImmutableList(), new Context() { Pause = null});

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputNumber), new List<Codel>().ToImmutableList(), new Context() { Pause = pauseActionMock.Object });

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputCharacter), new List<Codel>().ToImmutableList(), new Context() { Pause = null });

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

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.InputCharacter), new List<Codel>().ToImmutableList(), new Context()
            {
                Pause = pauseActionMock.Object
            });

        inputServiceMock.Verify(service => service.RequestCharacterInputAsync(), Times.Once);
        pauseActionMock.Verify(action => action.Invoke(), Times.Once);
    }


    [Fact]
    public void OutputNumber_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;

        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(
            new ColorCommand(PietColors.Blue, Command.Command.OutputNumber),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Fact]
    public void OutputNumber_StackHasSufficientValues_MustMatch()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(42), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.OutputNumber), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Empty(currentProgramStackAsArray);
    }

    [Fact]
    public void OutputCharacter_StackIsEmpty_ThrowsInternalException_MustInvoke_OnErrorAction()
    {
        _onErrorActionWasTriggered = false;
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);


        programOperator.ExecuteCommand(
            new ColorCommand(PietColors.Blue, Command.Command.OutputCharacter),
            new List<Codel>().ToImmutableList(), new Context { OnError = TestOnError });

        Assert.True(_onErrorActionWasTriggered);
    }

    [Fact]
    public void OutputCharacter_StackHasSufficientValues_MustMatch()
    {
        var outputEventServiceMock = new Mock<IOutputService>();
        var inputServiceMock = new Mock<IInputService>();

        var programOperator = new ProgramOperator(new NullLogger<ProgramOperator>(),
            outputEventServiceMock.Object, inputServiceMock.Object);

        programOperator.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push), GetFakeCodelBlockOfSize(1), null);
        programOperator.ExecuteCommand(new ColorCommand(PietColors.Blue, Command.Command.OutputCharacter), new List<Codel>().ToImmutableList(), null);

        var currentProgramStackAsArray = programOperator.GetProgramStack();

        Assert.Empty(currentProgramStackAsArray);
    }

}