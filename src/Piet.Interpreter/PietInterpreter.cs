using Microsoft.Extensions.Logging;
using Piet.Command;
using Piet.Grid;
using Piet.Interpreter.Output;

namespace Piet.Interpreter;

public sealed class PietInterpreter
{
    private EventHandler<InterpreterExceptionEventArgs>? PublishException;
    private readonly ICodelChooser _codelChooser;
    private readonly ICodelBlockSearcher _codelBlockSearcher;
    private readonly IProgramOperator _programOperator;
    private readonly ILogger<PietInterpreter> _logger;

    private Codel _currentCodel = null!;
    private bool _initialized;
    private State _state;

    internal static Direction DirectionPointer = Direction.Right;
    internal static CodelChooser CodelChooserState = CodelChooser.Left;

    public PietInterpreter(
        ILogger<PietInterpreter> logger,
        ICodelChooser codelChooser,
        ICodelBlockSearcher codelBlockSearcher,
        IProgramOperator programOperator
    )
    {
        _logger = logger;
        _programOperator = programOperator;
        _codelChooser = codelChooser;
        _codelBlockSearcher = codelBlockSearcher;
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
            _state = State.Completed;
            return;
        }

        _logger.LogDebug($"Get command for current color: {_currentCodel.Color} and next color: {nextCodelResult.Codel.Color}");
        var colorCommand = ColorCommandControl.GetColorCommand(_currentCodel.Color, nextCodelResult.Codel.Color);
        _logger.LogDebug($"Retrieved command is: {colorCommand}");
        
        if (nextCodelResult.TraversedWhiteCodels is false)
        {
            _logger.LogDebug($"Execute command: {colorCommand}");
            _programOperator.ExecuteCommand(colorCommand, codelBock.Count, new Context()
                {
                    Pause = PauseRequested,
                    OnError = OnError,
                });
            _logger.LogDebug($"Executed command: {colorCommand}");
        }
        
        _logger.LogDebug("Update current codel for next step");
        UpdateCurrentCodel(nextCodelResult.Codel);
    }

    private void PauseRequested()
    {
        Pause();
    }

    private void OnError()
    {
        Terminate();
    }

    public PietInterpreterResult Run(ICodelGrid codelGrid)
    {
        Start(codelGrid);
        Continue(codelGrid);
        return Complete();
    }

    public PietInterpreterResult Continue(ICodelGrid codelGrid, int input, Command.Command command)
    {
        _state = State.Running;
        _programOperator.SetInputValue(input, new ColorCommand(_currentCodel.Color, command));
        Continue(codelGrid);
        return Complete();
    }

    public PietInterpreterResult Terminate()
    {
        _state = State.Failed;
        return Complete();
    }

    private void Start(ICodelGrid codelGrid)
    {
        if (_initialized) return;

        _currentCodel = codelGrid.GetCodel(0, 0);
        _codelChooser.CodelGrid = codelGrid;
        _codelBlockSearcher.CodelGrid = codelGrid;
        _programOperator.Reset();

        DirectionPointer = Direction.Right;
        CodelChooserState = CodelChooser.Left;
        _state = State.Running;
        _initialized = true;
    }
    private void Pause()
    {
        _state = State.Paused;
    }

    private void Continue(ICodelGrid codelGrid)
    {
        _state = State.Running;
        _codelChooser.CodelGrid = codelGrid;
        _codelBlockSearcher.CodelGrid = codelGrid;
        
        _logger.LogInformation("Begin interpreting codel grid.");
        while (_state == State.Running)
        {
            NextStep();
        }
    }

    private PietInterpreterResult Complete()
    {
        if (_state == State.Completed || _state == State.Failed)
        {
            _initialized = false;
        }
        return new(_state, GetResultMessage(_state));
    }

    private static string GetResultMessage(State state) =>
        state switch
        {
            State.Completed => "Successfully interpreted codel grid",
            State.Failed => "An error occurred. The interpreter stopped.",
            State.Paused => "Interpreter paused",
            State.Running => "Interpreter is running"
        };

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