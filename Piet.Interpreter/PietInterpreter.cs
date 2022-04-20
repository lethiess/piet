using Microsoft.Extensions.Logging;
using Piet.Command;
using Piet.Grid;

namespace Piet.Interpreter;

public sealed class PietInterpreter
{
    private readonly ICodelChooser _codelChooser;
    private readonly ICodelBlockSearcher _codelBlockSearcher;
    private readonly ILogger<PietInterpreter> _logger;
    
    private readonly Stack<int> _programStack;
    private Codel _currentCodel;
    private bool _executionFinished = false;
    private bool _executionError = false;

    internal static Direction DirectionPointer = Direction.Right;
    internal static CodelChooser CodelChooserState = CodelChooser.Left;
    
    public PietInterpreter(
        ILogger<PietInterpreter> logger,
        ICodelGrid codelGrid,
        ICodelChooser codelChooser,
        ICodelBlockSearcher codelBlockSearcher
    )
    {
        _logger = logger;
        _codelChooser = codelChooser;
        _codelBlockSearcher = codelBlockSearcher;
        _programStack = new();
        _currentCodel = codelGrid.GetCodel(0, 0);
    }

    public void Run()
    {
        DirectionPointer = Direction.Right;
        CodelChooserState = CodelChooser.Left;

        _logger.LogInformation("Begin interpreting codel grid.");
        while (_executionFinished is false && _executionError is false)
        {
            NextStep();
        }

        if (_executionFinished && _executionError is false)
        {
            _logger.LogInformation("Successfully interpreted codel grid");
        }
        else
        {
            _logger.LogCritical("An error occured - Piet interpreter stopped");
        }
    }

    private void NextStep()
    {
        _logger.LogDebug("Get codel block for current codel: {_currentCodel}", _currentCodel);
        var codelBock = _codelBlockSearcher.GetCodelBock(_currentCodel).ToList();
        _logger.LogDebug($"Retrived codel block of size {codelBock.Count()}");
        
        _logger.LogDebug("Determine next codel");
        var nextCodelResult = _codelChooser.GetNextCodel(codelBock);
        _logger.LogDebug($"Next codel: {nextCodelResult.Codel}");

        if (nextCodelResult.Codel is null)
        {
            _logger.LogDebug("Program terminates because there is no next codel");
            _executionFinished = true;
            return;
        }

        _logger.LogDebug($"Get command for current color: {_currentCodel.Color} and next color: {nextCodelResult.Codel.Color}");
        var colorCommand = ColorCommandControl.GetColorCommand(_currentCodel.Color, nextCodelResult.Codel.Color);
        _logger.LogDebug($"Retrived command is: {colorCommand}");
        
        if (nextCodelResult.TraversedWhiteCodels is false)
        {
            _logger.LogDebug($"Execute command: {colorCommand}");
            ExecuteCommand(colorCommand, codelBock.Count());
            _logger.LogDebug($"Executed command: {colorCommand}");
        }
        
        _logger.LogDebug("Update current codel for next step");
        UpdateCurrentCodel(nextCodelResult.Codel);
    }

    private void UpdateCurrentCodel(Codel codel) => _currentCodel = codel;

    private void ExecuteCommand(ColorCommand command, int codelBlockSize)
    {
        int operand;
        int operandA;
        int operandB;

        switch (command.Command)
        {
            case Command.Command.None:
                _logger.LogDebug($"Executing command {Command.Command.None}: program state does not change");
                break;
            case Command.Command.Push:
                _programStack.Push(codelBlockSize);
                break;
            case Command.Command.Pop:
                _programStack.Pop();
                break;
            case Command.Command.Add:
                if (_programStack.Count < 2)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operandB = _programStack.Pop();
                operandA = _programStack.Pop();
                _programStack.Push(operandA + operandB);
                break;
            case Command.Command.Subtract:
                if (_programStack.Count < 2)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operandB = _programStack.Pop();
                operandA = _programStack.Pop();
                _programStack.Push(operandA - operandB);
                break;
            case Command.Command.Multiply:
                if (_programStack.Count < 2)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operandB = _programStack.Pop();
                operandA = _programStack.Pop();
                _programStack.Push(operandA * operandB);
                break;
            case Command.Command.Divide:
                if (_programStack.Count < 2)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operandB = _programStack.Pop();
                operandA = _programStack.Pop();

                if (operandB == 0)
                {
                    throw new PietInterpreterDividedByZeroException("Division by zero is undefined.");
                }

                _programStack.Push(operandA / operandB);
                break;
            case Command.Command.Modulo:
                if (_programStack.Count < 2)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operandB = _programStack.Pop();
                operandA = _programStack.Pop();

                if (operandB == 0)
                {
                    throw new PietInterpreterDividedByZeroException("Modulo division for zero is undefined.");
                }

                var result = operandA % operandB;
                if (result < 0 && operandB > 0)
                {
                    result = Math.Abs(result);
                }
                _programStack.Push(result);
                break;
            case Command.Command.Not:
                if (_programStack.Count < 1)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operand = _programStack.Pop();
                if (operand == 0)
                {
                    _programStack.Push(1);
                }
                else
                {
                    _programStack.Push(0);
                }

                break;
                
            case Command.Command.Greater:
                if (_programStack.Count < 2)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operandB = _programStack.Pop();
                operandA = _programStack.Pop();

                _programStack.Push(operandA > operandB ? 1 : 0);
                break;

            case Command.Command.Pointer:
                if (_programStack.Count < 1)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operand = _programStack.Pop();

                if (operand > 0)
                {
                    for (int i = 0; i < operand; i++)
                    {
                        RotateDirectionPointerClockwise();
                    }
                }

                if (operand < 0)
                {
                    for (int i = 0; i < Math.Abs(operand); i++)
                    {
                        RotateDirectionPointerCounterClockwise();
                    }
                }
                break;

            case Command.Command.Switch:
                if (_programStack.Count < 1)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operand = _programStack.Pop();

                for (int i = 0; i <= Math.Abs(operand); i++)
                {
                    ToggleCodelChooser();
                }

                break;

            case Command.Command.Duplicate:
                if (_programStack.Count < 1)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operand = _programStack.Peek();
                _programStack.Push(operand);
                break;
            
            case Command.Command.Roll:
                if (_programStack.Count < 2)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                var numberOfRolls = _programStack.Pop();
                var depthOfRollOperation = _programStack.Pop();

                // convert stack to array to perform roll operation
                var stackAsArray = _programStack.ToArray();
                Array.Reverse(stackAsArray);

                if (depthOfRollOperation < stackAsArray.Length)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"Error in 'roll operation': There are {_programStack.Count} elements on the stack" +
                        $"but a roll depth of {depthOfRollOperation} was requested.");
                }

                // roll
                int rollInsertIndex = stackAsArray.Length - depthOfRollOperation;
                for (int i = 0; i < numberOfRolls; i++)
                {
                    int programStackTopElement = stackAsArray[^1];
                    Array.Copy(stackAsArray, rollInsertIndex, stackAsArray, rollInsertIndex + 1, stackAsArray.Length - rollInsertIndex - 1);
                    stackAsArray.SetValue(programStackTopElement, rollInsertIndex);
                }
                
                // back to stack
                _programStack.Clear();
                foreach (var number in stackAsArray)
                {
                    _programStack.Push(number);
                }
                
                break;

            case Command.Command.InputNumber:
                // TODO:
                break;
            case Command.Command.InputCharacter:
                // TODO:
                break;
            case Command.Command.OutputNumber:
                if (_programStack.Count < 1)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operand = _programStack.Pop();

                _logger.LogInformation($"{Convert.ToChar(operand)}");
                // TODO: emit event

                break;
            case Command.Command.OutputCharacter:
                if (_programStack.Count < 1)
                {
                    throw new InsufficientNumberOfElementsOnProgramStackException($"There are {_programStack.Count} elements on the stack");
                }

                operand = _programStack.Pop();
                _logger.LogInformation($"{Convert.ToChar(operand)}");
                // TODO: emit event


                break;
            default:
                throw new ArgumentOutOfRangeException(
                    $"The command ${command.Command} is not valid in this context");
        }
    }


    internal static void ToggleCodelChooser()
    {
        CodelChooserState = CodelChooserState switch
        {
            CodelChooser.Left => CodelChooser.Right,
            CodelChooser.Right => CodelChooser.Left,
            _ => throw new ArgumentOutOfRangeException($"The value {CodelChooserState} of type {typeof(CodelChooser)} is invalid in this context")
        };
    }

    internal static void RotateDirectionPointerClockwise()
    {
        DirectionPointer = DirectionPointer switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(
                $"The value {DirectionPointer} of type {typeof(Direction)} is invalid in this context")
        };
    }

    internal static void RotateDirectionPointerCounterClockwise()
    {
        DirectionPointer = DirectionPointer switch
        {
            Direction.Up => Direction.Left,
            Direction.Left => Direction.Down,
            Direction.Down => Direction.Right,
            Direction.Right => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(
                $"The value {DirectionPointer} of type {typeof(Direction)} is invalid in this context")
        };
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public enum CodelChooser
    {
        Left,
        Right
    }
}