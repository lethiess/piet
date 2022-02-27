using Microsoft.Extensions.Logging;
using Piet.Command;
using Piet.Grid;

namespace Piet.Interpreter;

public sealed class PietInterpreter
{
    private readonly ICodelGrid _codelGrid;
    private readonly ICodelChooser _codelChooser;
    private readonly ILogger<PietInterpreter> _logger;
    private readonly Stack<int> _programStack;
    private Codel _currentCodel;
    private bool _executionFinished = false;
    private bool _executionError = false;

    public PietInterpreter(
        ICodelGrid codelGrid,
        ICodelChooser codelChooser,
        ILogger<PietInterpreter> logger
    )
    {
        _codelGrid = codelGrid;
        _codelChooser = codelChooser;
        _logger = logger;
        _programStack = new();
        _currentCodel = _codelGrid.GetCodel(0, 0);
    }

    public void Run()
    {
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
        _logger.LogDebug($"Get codel block for current codel: {_currentCodel}");
        var codelBock = CodelBockSearcher.GetCodelBock(_currentCodel, _codelGrid).ToList();
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
    
}