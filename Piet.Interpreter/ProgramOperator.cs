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
            switch (colorCommand.Command)
            {
                case Command.Command.None: None(); break;
                case Command.Command.Push: Push(codelBlockSize); break;
                case Command.Command.Pop: Pop(); break;
                case Command.Command.Add: Add(); break;
                case Command.Command.Subtract: Subtract(); break;
                case Command.Command.Multiply: Multiply(); break;
                case Command.Command.Divide: Divide(); break;
                case Command.Command.Modulo: Modulo(); break;
                case Command.Command.Not: Not(); break;
                case Command.Command.GreaterThan: GreaterThan(); break;
                case Command.Command.Pointer: Pointer(); break;
                case Command.Command.Switch: Switch(); break;
                case Command.Command.Duplicate: Duplicate(); break;
                case Command.Command.Roll: Roll(); break;
                case Command.Command.InputNumber: InputNumber(); break;
                case Command.Command.InputCharacter: InputCharacter(); break;
                case Command.Command.OutputNumber: OutputNumber(); break;
                case Command.Command.OutputCharacter: OutputCharacter(); break;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"The command ${colorCommand.Command} is not valid in this context");
            }
        }


        private void None()
        {
            _logger.LogDebug(
                $"Executing command {Command.Command.None}: Program state does not change.");
        }

        private void Push(int codelBlockSize)
        {
            _logger.LogDebug(
                $"Executing command {Command.Command.Push}: Push {codelBlockSize} on the stack.");
            _programStack.Push(codelBlockSize);
        }

        private void Pop()
        {
            if (_programStack.Count == 0)
            {
                _logger.LogDebug(
                    $"Executing command {Command.Command.Pop}: Program stack is empty, can not pop a element.");
                return;
            }

            _logger.LogDebug(
                $"Executing command {Command.Command.Pop}: Pop {_programStack.Peek()} from the stack.");
            _programStack.Pop();
        }

        private void Add()
        {
            if (_programStack.Count < 2)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();
            _programStack.Push(operandA + operandB);
        }

        private void Subtract()
        {
            if (_programStack.Count < 2)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();
            _programStack.Push(operandA - operandB);
        }

        private void Multiply()
        {
            if (_programStack.Count < 2)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();
            _programStack.Push(operandA * operandB);
        }

        private void Divide()
        {
            if (_programStack.Count < 2)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();

            if (operandB == 0)
            {
                throw new PietInterpreterDividedByZeroException("Division by zero is undefined.");
            }

            _programStack.Push(operandA / operandB);
        }

        private void Modulo()
        {
            if (_programStack.Count < 2)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();

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
        }

        private void Not()
        {
            if (_programStack.Count < 1)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operand = _programStack.Pop();
            if (operand == 0)
            {
                _programStack.Push(1);
            }
            else
            {
                _programStack.Push(0);
            }
        }

        private void GreaterThan()
        {
            if (_programStack.Count < 2)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();

            _programStack.Push(operandA > operandB ? 1 : 0);
        }

        private void Pointer()
        {
            if (_programStack.Count < 1)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operand = _programStack.Pop();

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
        }

        private void Switch()
        {
            if (_programStack.Count < 1)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operand = _programStack.Pop();

            for (int i = 0; i <= Math.Abs(operand); i++)
            {
                PietInterpreter.ToggleCodelChooser();
            }
        }

        private void Duplicate()
        {
            if (_programStack.Count < 1)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operand = _programStack.Peek();
            _programStack.Push(operand);
        }

        private void Roll()
        {
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
        }

        private void InputNumber()
        {
            int inputNumber = _inputService.GetIntegerInput().Result;
            _programStack.Push(inputNumber);
        }

        private void InputCharacter()
        {
            int inputCharacter = _inputService.GetCharacterInput().Result;
            _programStack.Push(inputCharacter);
        }

        private void OutputNumber()
        {
            if (_programStack.Count < 1)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operand = _programStack.Pop();
            _logger.LogDebug($"Numeric output value {operand}");
            _outputEventService.DispatchOutputIntegerEvent(operand);
        }

        private void OutputCharacter()
        {
            if (_programStack.Count < 1)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"There are {_programStack.Count} elements on the stack");
            }

            var operand = _programStack.Pop();
            _logger.LogDebug($"Character output value{Convert.ToChar(operand)}");
            _outputEventService.DispatchOutputCharacterEvent((char)operand);
        }
    }
}