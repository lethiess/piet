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

        public ProgramOperator(ILogger<ProgramOperator> logger,
            IOutputService outputService, IInputService inputService)
        {
            _programStack = new Stack<int>();
            _logger = logger;
            OutputService = outputService;
            InputService = inputService;
        }

        internal List<int> GetProgramStack() => _programStack.ToList();

        public IInputService InputService { get; init; }
    
        public IOutputService OutputService { get; init; }

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
            if (result < 0 || operandB > 0)
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

                for (int i = 0; i < operand % 4; i++)
                {
                    PietInterpreter.RotateDirectionPointerClockwise();
                }
            }

            if (operand < 0)
            {
                for (int i = 0; i < Math.Abs(operand) % 4; i++)
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

            // Toggle the direction only if there is a chance
            if (Math.Abs(operand) % 2 == 1)
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

            var numberOfRolls = Math.Abs(_programStack.Pop()); // ignore negative rolls
            var depthOfRollOperation = _programStack.Pop();

            if (depthOfRollOperation < 0)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException("Negative depths for the roll operations are not allowed.");
            }

            // convert stack to array to perform roll operation
            var stackAsArray = _programStack.ToArray();
            Array.Reverse(stackAsArray);

            if (depthOfRollOperation > stackAsArray.Length)
            {
                throw new InsufficientNumberOfElementsOnProgramStackException(
                    $"Error in 'roll operation': There are {_programStack.Count} elements on the stack" +
                    $"but a roll depth of {depthOfRollOperation} was requested.");
            }

            // perform actual roll operation
            int rollInsertIndex = stackAsArray.Length - depthOfRollOperation - 1;
            for (int i = 0; i < numberOfRolls; i++)
            {
                int programStackTopElement = stackAsArray[^1];
                Array.Copy(stackAsArray, rollInsertIndex, stackAsArray, rollInsertIndex + 1,
                    stackAsArray.Length - rollInsertIndex - 1);
                stackAsArray.SetValue(programStackTopElement, rollInsertIndex);
            }

            // convert array back to stack
            _programStack.Clear();
            foreach (var number in stackAsArray)
            {
                _programStack.Push(number);
            }
        }

        private void InputNumber()
        {
            int inputNumber = InputService.GetIntegerInput().Result;
            _programStack.Push(inputNumber);
        }

        private void InputCharacter()
        {
            int inputCharacter = InputService.GetCharacterInput().Result;
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
            OutputService.DispatchOutputIntegerEvent(operand);
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
            OutputService.DispatchOutputCharacterEvent((char)operand);
        }
    }
}