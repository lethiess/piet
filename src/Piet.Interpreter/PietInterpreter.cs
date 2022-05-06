using Microsoft.Extensions.Logging;
using Piet.Command;
using Piet.Grid;

namespace Piet.Interpreter;

public sealed class PietInterpreter
{
    private readonly ICodelChooser _codelChooser;
    private readonly ICodelBlockSearcher _codelBlockSearcher;
    private readonly IProgramOperator _programOperator;
    private readonly ILogger<PietInterpreter> _logger;
    
    private Codel _currentCodel;
    private bool _executionFinished = false;
    private bool _executionError = false;

    internal static Direction DirectionPointer = Direction.Right;
    internal static CodelChooser CodelChooserState = CodelChooser.Left;
    
    public PietInterpreter(
        ILogger<PietInterpreter> logger,
        IProgramOperator programOperator,
        ICodelChooser codelChooser,
        ICodelBlockSearcher codelBlockSearcher
    )
    {
        _logger = logger;
        _programOperator = programOperator;
        _codelChooser = codelChooser;
        _codelBlockSearcher = codelBlockSearcher;
    }

    public PietInterpreterResult Run(ICodelGrid codelGrid)
    {
        _currentCodel = codelGrid.GetCodel(0, 0);
        _codelChooser.CodelGrid = codelGrid;
        _codelBlockSearcher.CodelGrid = codelGrid;


        DirectionPointer = Direction.Right;
        CodelChooserState = CodelChooser.Left;

        _logger.LogInformation("Begin interpreting codel grid.");
        while (_executionFinished is false && _executionError is false)
        {
            NextStep();
        }

        if (_executionFinished && _executionError is false)
        {
            return new PietInterpreterResult(PietInterpreterResult.InterpreterStatus.Success, "Successfully interpreted codel grid");
        }

        return new PietInterpreterResult(PietInterpreterResult.InterpreterStatus.Error, "An error occurred. The interpreter stopped.");
    }

    private void NextStep()
    {
        _logger.LogDebug("Get codel block for current codel: {_currentCodel}", _currentCodel);
        var codelBock = _codelBlockSearcher.GetCodelBock(_currentCodel).ToList();
        _logger.LogDebug($"Retrieved codel block of size {codelBock.Count}");
        
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
            _programOperator.ExecuteCommand(colorCommand, codelBock.Count);
            _logger.LogDebug($"Executed command: {colorCommand}");
        }
        
        _logger.LogDebug("Update current codel for next step");
        UpdateCurrentCodel(nextCodelResult.Codel);
    }

    private void UpdateCurrentCodel(Codel codel) => _currentCodel = codel;
    
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