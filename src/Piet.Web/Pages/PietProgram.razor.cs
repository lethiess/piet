using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Piet.Color;
using Piet.Command;
using Piet.Grid;
using Piet.Interpreter;
using Piet.Interpreter.Output;
using Piet.Web.Shared;

namespace Piet.Web.Pages;

public partial class PietProgram : IDisposable
{
    [CascadingParameter] 
    public IModalService ModalService { get; set; } = default!;

    [Inject] 
    private ILoggerFactory LoggerFactory { get; init; } = default!;

    [Inject] 
    private IProgramOperator ProgramOperator { get; init; } = default!;

    [Inject]
    private ICodelChooser CodelChooser { get; init; } = default!;

    [Inject]
    private ICodelBlockSearcher CodelBlockSearcher { get; init; } = default!;

    [Inject]
    private ILogger<PietProgram> Logger { get; init; } = default!;

    private PietInterpreter _interpreter = null!;

    private const int InitialGridHeight = 15;
    private const int InitialGridWidth = 25;

    private int _gridHeight = InitialGridHeight;
    private int _gridWidth = InitialGridWidth;
    private PietColor _currentColor = PietColors.LightRed;
    private static readonly PietColor s_colorBlack = PietColors.Black;
    private static readonly PietColor s_colorWhite = PietColors.White;

    private List<string> _output = new();
    private List<CommandInfo> _commandHistory = new();

    private CodelGrid _codelGrid = null!;
    private ColorCommand[,] _colorCommands = null!;
    private Stack<int>? _programStack = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        RegisterEventListeners();

        _codelGrid = new CodelGridBuilder()
            .WithHeight(_gridHeight)
            .WithWidth(_gridWidth)
            .WithInitialColor(PietColors.White)
            .Build();

        _colorCommands =
            ColorCommandControl.GetColorCommands(_currentColor);

        _interpreter = new PietInterpreter(
            LoggerFactory.CreateLogger<PietInterpreter>(),
            CodelChooser,
            CodelBlockSearcher,
            ProgramOperator
        );
    }

    private void UpdateColor(int xPosition, int yPosition)
    {
        Logger.LogDebug("UpdateColor xPosition: {X} yPosition: {Y}", xPosition, yPosition);
        _codelGrid.SetCodelColor(xPosition, yPosition, _currentColor);
    }

    private void UpdateColorCommand(int saturationIndex, int hueIndex)
    {
        Logger.LogDebug("UpdateColorCommand saturationIndex: {Sat} hueIndex: {Hue}", saturationIndex, hueIndex);
        _currentColor = _colorCommands[saturationIndex, hueIndex].Color;
        _colorCommands = ColorCommandControl.GetColorCommands(_currentColor);
    }

    private void SetCurrentColor(PietColor color)
    {
        _currentColor = color;
    }

    private void Reset()
    {
        _gridHeight = InitialGridHeight;
        _gridWidth = InitialGridWidth;
        _currentColor = PietColors.LightRed;

        _codelGrid = new CodelGridBuilder()
            .WithHeight(_gridHeight)
            .WithWidth(_gridWidth)
            .WithInitialColor(PietColors.White)
            .Build();

        _colorCommands = ColorCommandControl.GetColorCommands(_currentColor);

        _output.Clear();
        _commandHistory.Clear();
    }

    private void ResizeGrid()
    {
        _codelGrid = new CodelGridBuilder()
            .WithHeight(_gridHeight)
            .WithWidth(_gridWidth)
            .WithInitialColor(PietColors.White)
            .Build();
    }

    private void FillGridRandom()
    {
        _codelGrid.FillWithRandomValues();
    }

    private string GetCellName(ColorCommand colorCommand)
    {
        if (_currentColor != s_colorWhite && _currentColor != s_colorBlack
            && colorCommand is not null && colorCommand.Command != Command.Command.None)
        {
            return colorCommand.Command.ToString();
        }
        return "";
    }

    private void Run()
    {
        _output.Clear();
        _commandHistory.Clear();
        var result = _interpreter.Run(_codelGrid);

        Logger.LogDebug("Interpreter result: {State} - {Message}", result.State, result.Message);
    }

    private void RegisterEventListeners()
    {
        ProgramOperator.OutputService.OutputInteger += OutputServiceOnOutputInteger;
        ProgramOperator.OutputService.OutputCharacter += OutputServiceOnOutputCharacter;
        ProgramOperator.InputService.IntegerRequest += InputServiceOnIntegerRequest;
        ProgramOperator.InputService.CharacterRequest += InputServiceOnInputCharacterRequest;
        ProgramOperator.OutputService.OutputCommandLog += OutputCommandLog;
        ProgramOperator.OutputService.InterpreterException += InterpreterException;
        ProgramOperator.OutputService.ProgramOperatorUpdate += ProgramOperatorUpdate;
    }

    private void UnregisterEventListeners()
    {
        ProgramOperator.OutputService.OutputInteger -= OutputServiceOnOutputInteger;
        ProgramOperator.OutputService.OutputCharacter -= OutputServiceOnOutputCharacter;
        ProgramOperator.InputService.IntegerRequest -= InputServiceOnIntegerRequest;
        ProgramOperator.InputService.CharacterRequest -= InputServiceOnInputCharacterRequest;
        ProgramOperator.OutputService.OutputCommandLog -= OutputCommandLog;
        ProgramOperator.OutputService.InterpreterException -= InterpreterException;
        ProgramOperator.OutputService.ProgramOperatorUpdate -= ProgramOperatorUpdate;
    }

    private void OutputCommandLog(object? sender, OutputCommandLogEventArg e)
    {
        _commandHistory.Add(e.CommandInfo);
        InvokeAsync(StateHasChanged);
    }

    private async void InterpreterException(object? sender, InterpreterExceptionEventArgs e)
    {
        try
        {
            await InvokeAsync(async () => await ShowErrorModal(e.Message));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error handling interpreter exception");
        }
    }

    private void ProgramOperatorUpdate(object? sender, ProgramOperatorUpdateEventArgs e)
    {
        _programStack = e.CurrentInterpreterStack;
        InvokeAsync(StateHasChanged);
    }

    private async void InputServiceOnIntegerRequest(object? sender, EventArgs e)
    {
        try
        {
            Logger.LogDebug("Input integer requested");
            await InvokeAsync(async () => await ShowModalForInteger());
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error handling integer input request");
        }
    }

    private async void InputServiceOnInputCharacterRequest(object? sender, EventArgs e)
    {
        try
        {
            Logger.LogDebug("Input character requested");
            await InvokeAsync(async () => await ShowModalForCharacter());
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error handling character input request");
        }
    }

    private void OutputServiceOnOutputCharacter(object? sender, OutputCharacterOperationEventArgs e)
    {
        Logger.LogDebug("Character output: {Value}", e.Value);
        _output.Add(e.Value.ToString());
        InvokeAsync(StateHasChanged);
    }

    private void OutputServiceOnOutputInteger(object? sender, OutputIntegerOperationEventArgs e)
    {
        Logger.LogDebug("Integer output: {Value}", e.Value);
        _output.Add(e.Value.ToString());
        InvokeAsync(StateHasChanged);
    }

    private async Task ShowModalForCharacter()
    {
        var messageForm = ModalService.Show<MessageFormCharacter>();
        var result = await messageForm.Result;

        if (result.Cancelled is false)
        {
            _interpreter.Continue(_codelGrid, result.Data.ToString()![0], Command.Command.InputCharacter);
        }
        else
        {
            _interpreter.Terminate();
        }
    }

    private async Task ShowModalForInteger()
    {
        var messageForm = ModalService.Show<MessageFormInteger>();
        var result = await messageForm.Result;

        if (result.Cancelled is false)
        {
            _interpreter.Continue(_codelGrid, (int)result.Data, Command.Command.InputNumber);
        }
        else
        {
            _interpreter.Terminate();
        }
    }

    private async Task ShowErrorModal(string message)
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(ErrorMessage.Message), message);
        var options = new ModalOptions()
        {
            DisableBackgroundCancel = true,
            Animation = ModalAnimation.FadeIn(2)
        };

        var messageForm = ModalService.Show<ErrorMessage>("Error", parameters, options);
        await messageForm.Result;

        _interpreter.Terminate();
    }

    internal static string Map(PietInterpreter.CodelChooser codelChooser) =>
        codelChooser switch
        {
            PietInterpreter.CodelChooser.Left => "\u2190",
            PietInterpreter.CodelChooser.Right => "\u2192",
            _ => ""
        };

    internal static string Map(PietInterpreter.Direction directionPointer) =>
        directionPointer switch
        {
            PietInterpreter.Direction.Down => "\u2193",
            PietInterpreter.Direction.Left => "\u2190",
            PietInterpreter.Direction.Up => "\u2191",
            PietInterpreter.Direction.Right => "\u2192",
            _ => ""
        };

    internal static string GetSerializedCommand(CommandInfo command)
    {
        string operandA = command.OperandA.HasValue ? command.OperandA.Value.ToString() : "NaN";
        string operandB = command.OperandB.HasValue ? command.OperandB.Value.ToString() : "NaN";
        string value = command.Value.HasValue ? command.Value.Value.ToString() : "NaN";

        if (command.ColorCommand.Command is Command.Command.OutputCharacter or Command.Command.InputCharacter
            && command.Value.HasValue)
        {
            value = ((char)command.Value).ToString();
        }
        
        return command.ColorCommand.Command switch
        {
            Command.Command.None => $"{command.ColorCommand.Command}",
            Command.Command.Roll => $"{command.ColorCommand.Command} (depth: {operandA}, rolls: {operandB})",
            Command.Command.Add => $"{command.ColorCommand.Command} ({operandA} + {operandB} = {value})",
            Command.Command.Subtract => $"{command.ColorCommand.Command} ({operandA} - {operandB} = {value})",
            Command.Command.Multiply => $"{command.ColorCommand.Command} ({operandA} * {operandB} = {value})",
            Command.Command.Divide => $"{command.ColorCommand.Command} ({operandA} / {operandB} = {value})",
            Command.Command.Modulo => $"{command.ColorCommand.Command} ({operandA} % {operandB} = {value})",
            _ => $"{command.ColorCommand.Command} ({value})"
        };
    }

    public void Dispose()
    {
        UnregisterEventListeners();
    }
}
