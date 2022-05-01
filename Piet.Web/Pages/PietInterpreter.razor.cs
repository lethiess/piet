using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Piet.Color;
using Piet.Command;
using Piet.Grid;
using Piet.Interpreter;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;
using Piet.Web.Shared;

namespace Piet.Web.Pages
{
    public partial class PietInterpreter
    {
        [CascadingParameter] 
        public IModalService Modal { get; set; } = default!;

        [Inject] 
        private ILoggerFactory LoggerFactory { get; init; } = default!;

        [Inject] 
        private IProgramOperator ProgramOperator { get; init; } = default!;

        [Inject]
        private ILogger<PietInterpreter> Logger { get; init; } = default!;

        private const int InitialGridHeight = 15;
        private const int InitialGridWidth = 25;

        private static int _gridHeight = InitialGridHeight;
        private static int _gridWidth = InitialGridWidth;
        private static PietColor _currentColor = PietColors.LightRed;
        private static PietColor _colorBlack = PietColors.Black;
        private static PietColor _colorWhite = PietColors.White;

        private List<string> _output = new();

        private static CodelGrid _codelGrid = null!;
        private static ColorCommand[,] _colorCommands = null!;

        public PietInterpreter()
        {
            _codelGrid = new CodelGridBuilder()
                .WithHeight(_gridHeight)
                .WithWidth(_gridWidth)
                .WithInitialColor(PietColors.White)
                .Build();
            _colorCommands =
                ColorCommandControl.GetColorCommands(_currentColor);

            //RegisterEventListener();
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

        private async Task Run()
        {
            Console.WriteLine("Run");

            RegisterEventListener();


            var interpreter = new Piet.Interpreter.PietInterpreter(
                LoggerFactory.CreateLogger<Piet.Interpreter.PietInterpreter>(),
                _codelGrid,
                new CodelChooser(_codelGrid),
                new CodelBlockSearcher(_codelGrid),
                ProgramOperator
                );

            var result = await Task.Run(() => interpreter.Run());


            Console.WriteLine(result.Status);
            Console.WriteLine(result.Message);
        }

        private void RegisterEventListener()
        {
            ProgramOperator.OutputService.OutputInteger += OutputServiceOnOutputInteger;
            ProgramOperator.OutputService.OutputCharacter += OutputServiceOnOutputCharacter;

            ProgramOperator.InputService.InputInteger += InputServiceOnInputInteger;
            ProgramOperator.InputService.InputCharacter += InputServiceOnInputCharacter;
        }

        private async void InputServiceOnInputInteger(object? sender, EventArgs e)
        {
            Logger.LogDebug("Input integer");
            await ShowModalForInteger();
        }

        private async void InputServiceOnInputCharacter(object? sender, EventArgs e)
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

        async Task ShowModalForCharacter()
        {
            var messageForm = Modal.Show<MessageFormCharacter>();
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                // TODO: send input to interpreters stack
                //_message = result.Data?.ToString() ?? string.Empty;
            }
        }


        async Task ShowModalForInteger()
        {
            var messageForm = Modal.Show<MessageFormInteger>();
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                // TODO: send input to interpreters stack
                //_message = result.Data?.ToString() ?? string.Empty;
            }
        }
    }
}
