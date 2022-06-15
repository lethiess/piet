using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Piet.Color;
using Piet.Command;
using Piet.Grid;
using Piet.Interpreter;
using Piet.Interpreter.Output;
using Piet.Web.Shared;

namespace Piet.Web.Pages
{
    public partial class PietProgram
    {
        [CascadingParameter] 
        public IModalService Modal { get; set; } = default!;

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

        private static int _gridHeight = InitialGridHeight;
        private static int _gridWidth = InitialGridWidth;
        private static PietColor _currentColor = PietColors.LightRed;
        private static PietColor _colorBlack = PietColors.Black;
        private static PietColor _colorWhite = PietColors.White;

        private List<string> _output = new();
        private List<CommandInfo> _commandHistory = new();

        private static CodelGrid _codelGrid = null!;
        private static ColorCommand[,] _colorCommands = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            RegisterEventListener();

            _codelGrid = new CodelGridBuilder()
                .WithHeight(_gridHeight)
                .WithWidth(_gridWidth)
                .WithInitialColor(PietColors.White)
                .Build();

            _colorCommands =
                ColorCommandControl.GetColorCommands(_currentColor);

            _interpreter = new Piet.Interpreter.PietInterpreter(
                LoggerFactory.CreateLogger<Piet.Interpreter.PietInterpreter>(),
                CodelChooser,
                CodelBlockSearcher,
                ProgramOperator
            );

        }

        private void UpdateColor(int xPosition, int yPosition)
        {
            Console.WriteLine($"xPosition: {xPosition} yPosition: {yPosition}");
            _codelGrid.SetCodelColor(xPosition, yPosition, _currentColor);
        }

        private void UpdateColorCommand(int satuationIndex, int hueIndex)
        {
            Console.WriteLine($"satuationIndex: {satuationIndex} hueIndex: {hueIndex}");
            _currentColor = _colorCommands[satuationIndex, hueIndex].Color;
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
            if (_currentColor != _colorWhite && _currentColor != _colorBlack && colorCommand.Command != Command.Command.None)
            {
                return colorCommand.Command.ToString();
            }
            return "";
        }

        private void Run()
        {
            _output.Clear();
            var result = _interpreter.Run(_codelGrid);

            Logger.LogDebug(result.State.ToString());
            Logger.LogDebug(result.Message);
        }

        private void RegisterEventListener()
        {
            ProgramOperator.OutputService.OutputInteger += OutputServiceOnOutputInteger;
            ProgramOperator.OutputService.OutputCharacter += OutputServiceOnOutputCharacter;

            ProgramOperator.InputService.IntegerRequest += InputServiceOnIntegerRequest;
            ProgramOperator.InputService.CharacterRequest += InputServiceOnInputCharacterRequest;

            ProgramOperator.OutputService.OutputCommandLog += OutputCommandLog;
            ProgramOperator.OutputService.OutputException += OutputException;
        }

        private void OutputCommandLog(object? sender, OutputCommandLogEventArg e)
        {
            _commandHistory.Add(e.CommandInfo);
            StateHasChanged();
        }

        private async void OutputException(object? sender, InterpreterExceptionEventArgs e)
        {
            await ShowErrorModal(e.Message);
        }

        private async void InputServiceOnIntegerRequest(object? sender, EventArgs e)
        {
            Logger.LogDebug("Input integer");
            await ShowModalForInteger();
        }

        private async void InputServiceOnInputCharacterRequest(object? sender, EventArgs e)
        {
            Logger.LogDebug("Input character");
            await ShowModalForCharacter();
        }

        private void OutputServiceOnOutputCharacter(object? sender, OutputCharacterOperationEventArgs e)
        {
            Console.WriteLine(e.Value);
            _output.Add(e.Value.ToString());
            StateHasChanged();
        }

        private void OutputServiceOnOutputInteger(object? sender, OutputIntegerOperationEventArgs e)
        {
            Console.WriteLine(e.Value);
            _output.Add(e.Value.ToString());
            StateHasChanged();
        }

        private async Task ShowModalForCharacter()
        {
            var messageForm = Modal.Show<MessageFormCharacter>();
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
            var messageForm = Modal.Show<MessageFormInteger>();
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
            parameters.Add(nameof(DisplayMessage.Message), message);
            
            var messageForm = Modal.Show<DisplayMessage>("Error", parameters);
            await messageForm.Result;

            _interpreter.Terminate();
        }
        

        private string GetSerializedCommand(CommandInfo command) =>
            command.ColorCommand.Command switch
            {
                Command.Command.None => $"{command.ColorCommand.Command})",
                Command.Command.Push => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.Pop => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.Switch => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.Pointer => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.InputCharacter => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.InputNumber=> $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.OutputCharacter => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.OutputNumber => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.Duplicate => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.Not => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.Greater => $"{command.ColorCommand.Command} ({command.Value})",
                Command.Command.Roll => $"{command.ColorCommand.Command} (depth: {command.OperandA}, rolls: {command.OperandB})",
                Command.Command.Add => $"{command.ColorCommand.Command} ({command.OperandA} + {command.OperandB} = {command.Value})",
                Command.Command.Subtract => $"{command.ColorCommand.Command} ({command.OperandA} - {command.OperandB} = {command.Value})",
                Command.Command.Multiply => $"{command.ColorCommand.Command} ({command.OperandA} * {command.OperandB} = {command.Value})",
                Command.Command.Divide => $"{command.ColorCommand.Command} ({command.OperandA} / {command.OperandB} = {command.Value})",
                Command.Command.Modulo => $"{command.ColorCommand.Command} ({command.OperandA} % {command.OperandB} = {command.Value})",
            };
    }
}