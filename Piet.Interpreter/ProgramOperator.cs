using Microsoft.Extensions.Logging;
using Piet.Command;
using Piet.Interpreter.Events;
using Piet.Interpreter.Exceptions;

namespace Piet.Interpreter
{
    class ProgramOperator : IProgramOperator
    {
        private readonly Stack<int> _programStack;
        private readonly ILogger<ProgramOperator> _logger;
        private readonly IOutputEventService _outputEventService;
        private readonly IInputService _inputService;

        public ProgramOperator(ILogger<ProgramOperator> logger,
            IOutputEventService outputEventService, IInputService inputService)
        {
            _programStack = new Stack<int>();
            _logger = logger;
            _outputEventService = outputEventService;
            _inputService = inputService;
        }

        // TODO: delete 
        public void TEST_TriggerOutputOperation(int value)
        {
            _outputEventService.DispatchOutputIntegerEvent(value);
        }

        // TODO: delete 
        public void TEST_TriggerOutputOperation(char value)
        {
            _outputEventService.DispatchOutputCharacterEvent(value);
        }

        public void ExecuteCommand(ColorCommand colorCommand, int codelBlockSize)
        {
            int operand;
            int operandA;
            int operandB;

            //var test = command.Command switch
            //{
            //    Command.Command.None => NoneOperation(),
            //    Command.Command.Add => Add(),
            //    _ => throw new Exception()
            //};
            // TODO: create service CommandExecutionService which handel all commands


            switch (colorCommand.Command)
            {
                case Command.Command.None:
                    _logger.LogDebug(
                        $"Executing command {Command.Command.None}: program state does not change");
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
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operandB = _programStack.Pop();
                    operandA = _programStack.Pop();
                    _programStack.Push(operandA + operandB);
                    break;
                case Command.Command.Subtract:
                    if (_programStack.Count < 2)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operandB = _programStack.Pop();
                    operandA = _programStack.Pop();
                    _programStack.Push(operandA - operandB);
                    break;
                case Command.Command.Multiply:
                    if (_programStack.Count < 2)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operandB = _programStack.Pop();
                    operandA = _programStack.Pop();
                    _programStack.Push(operandA * operandB);
                    break;
                case Command.Command.Divide:
                    if (_programStack.Count < 2)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operandB = _programStack.Pop();
                    operandA = _programStack.Pop();

                    if (operandB == 0)
                    {
                        throw new PietInterpreterDividedByZeroException(
                            "Division by zero is undefined.");
                    }

                    _programStack.Push(operandA / operandB);
                    break;
                case Command.Command.Modulo:
                    if (_programStack.Count < 2)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operandB = _programStack.Pop();
                    operandA = _programStack.Pop();

                    if (operandB == 0)
                    {
                        throw new PietInterpreterDividedByZeroException(
                            "Modulo division for zero is undefined.");
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
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
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
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operandB = _programStack.Pop();
                    operandA = _programStack.Pop();

                    _programStack.Push(operandA > operandB ? 1 : 0);
                    break;

                case Command.Command.Pointer:
                    if (_programStack.Count < 1)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operand = _programStack.Pop();

                    if (operand > 0)
                    {
                        for (int i = 0; i < operand; i++)
                        {
                            PietInterpreter.RotateDirectionPointerClockwise();
                        }
                    }

                    if (operand < 0)
                    {
                        for (int i = 0; i < Math.Abs(operand); i++)
                        {
                            PietInterpreter.RotateDirectionPointerCounterClockwise();
                        }
                    }

                    break;

                case Command.Command.Switch:
                    if (_programStack.Count < 1)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operand = _programStack.Pop();

                    for (int i = 0; i <= Math.Abs(operand); i++)
                    {
                        PietInterpreter.ToggleCodelChooser();
                    }

                    break;

                case Command.Command.Duplicate:
                    if (_programStack.Count < 1)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operand = _programStack.Peek();
                    _programStack.Push(operand);
                    break;

                case Command.Command.Roll:
                    if (_programStack.Count < 2)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    var numberOfRolls = _programStack.Pop();
                    var depthOfRollOperation = _programStack.Pop();

                    // convert stack to array to perform roll operation
                    var stackAsArray = _programStack.ToArray();
                    Array.Reverse(stackAsArray);

                    if (depthOfRollOperation < stackAsArray.Length)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"Error in 'roll operation': There are {_programStack.Count} elements on the stack" +
                            $"but a roll depth of {depthOfRollOperation} was requested.");
                    }

                    // roll
                    int rollInsertIndex = stackAsArray.Length - depthOfRollOperation;
                    for (int i = 0; i < numberOfRolls; i++)
                    {
                        int programStackTopElement = stackAsArray[^1];
                        Array.Copy(stackAsArray, rollInsertIndex, stackAsArray, rollInsertIndex + 1,
                            stackAsArray.Length - rollInsertIndex - 1);
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
                    int inputNumber = _inputService.GetIntegerInput().Result;
                    _programStack.Push(inputNumber);
                    break;
                case Command.Command.InputCharacter:
                    int inputCharacter = _inputService.GetCharacterInput().Result;
                    _programStack.Push(inputCharacter);
                    break;
                case Command.Command.OutputNumber:
                    if (_programStack.Count < 1)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operand = _programStack.Pop();
                    _logger.LogDebug($"Numeric output value {operand}");
                    _outputEventService.DispatchOutputIntegerEvent(operand);

                    break;
                case Command.Command.OutputCharacter:
                    if (_programStack.Count < 1)
                    {
                        throw new InsufficientNumberOfElementsOnProgramStackException(
                            $"There are {_programStack.Count} elements on the stack");
                    }

                    operand = _programStack.Pop();
                    _logger.LogDebug($"Character output value{Convert.ToChar(operand)}");
                    _outputEventService.DispatchOutputCharacterEvent((char)operand);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"The command ${colorCommand.Command} is not valid in this context");

            }
        }
    }
}
