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
        var nextCodel = _codelChooser.GetNextCodel(codelBock);
        _logger.LogDebug($"Next codel: {nextCodel}");
        
        _logger.LogDebug($"Get command for current color: {_currentCodel.Color} and next color: {nextCodel.Color}");
        var colorCommand = ColorCommandControl.GetColorCommand(_currentCodel.Color, nextCodel.Color);
        _logger.LogDebug($"Retrived command is: {colorCommand}");
        
        _logger.LogDebug($"Execute command: {colorCommand}");
        ExecuteCommand(colorCommand, codelBock.Count());
        _logger.LogDebug($"Executed command: {colorCommand}");
        
        _logger.LogDebug("Update current codel for next step");
        UpdateCurrentCodel(nextCodel);
    }

    private void UpdateCurrentCodel(Codel codel) => _currentCodel = codel;

    private void ExecuteCommand(ColorCommand command, int codelBlockSize)
    {
        // todo: execute command
        throw new NotImplementedException();
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