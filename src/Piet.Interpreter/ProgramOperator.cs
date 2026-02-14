using System.Collections.Immutable;
using Microsoft.Extensions.Logging;
using Piet.Command;
using Piet.Grid;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter;

internal sealed class ProgramOperator : IProgramOperator
{
        private readonly Stack<int> _programStack;
        private readonly ILogger<ProgramOperator> _logger;
        private CommandInfo? _currentCommandInfo;

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

        public void SetInputValue(int input, ColorCommand colorCommand)
        {
            _programStack.Push(input);
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = input;
                LogCommand(_currentCommandInfo);
            }
        }

        public void Reset()
        {
            _programStack.Clear();
        }

        public void ExecuteCommand(ColorCommand colorCommand, ImmutableList<Codel> codelBlock, Context context)
        {
            _currentCommandInfo =
                new CommandInfo { ColorCommand = colorCommand, CodelBlock = codelBlock };
            Execute(colorCommand, codelBlock.Count, context);
            LogCommand(_currentCommandInfo);
        }

        private void Execute(ColorCommand colorCommand, int codelBlockSize, Context context)
        {
            try
            {
                switch (colorCommand.Command)
                {
                    case Command.Command.None:
                        None();
                        break;
                    case Command.Command.Push:
                        Push(codelBlockSize);
                        break;
                    case Command.Command.Pop:
                        Pop();
                        break;
                    case Command.Command.Add:
                        Add();
                        break;
                    case Command.Command.Subtract:
                        Subtract();
                        break;
                    case Command.Command.Multiply:
                        Multiply();
                        break;
                    case Command.Command.Divide:
                        Divide();
                        break;
                    case Command.Command.Modulo:
                        Modulo();
                        break;
                    case Command.Command.Not:
                        Not();
                        break;
                    case Command.Command.Greater:
                        GreaterThan();
                        break;
                    case Command.Command.Pointer:
                        Pointer();
                        break;
                    case Command.Command.Switch:
                        Switch();
                        break;
                    case Command.Command.Duplicate:
                        Duplicate();
                        break;
                    case Command.Command.Roll:
                        Roll();
                        break;
                    case Command.Command.InputNumber:
                        InputNumberAsync(context);
                        break;
                    case Command.Command.InputCharacter:
                        InputCharacterAsync(context);
                        break;
                    case Command.Command.OutputNumber:
                        OutputNumber();
                        break;
                    case Command.Command.OutputCharacter:
                        OutputCharacter();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"The command ${colorCommand.Command} is not valid in this context");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error executing command {Command}", colorCommand.Command);
            }
            OutputService.DispatchOutputProgramOperatorUpdateEvent(_programStack);
        }

        private void LogCommand(CommandInfo info)
        {
            OutputService.DispatchOutputCommandLogEvent(info);
        }

        private void None()
        {
            _logger.LogDebug(
                "Executing command {Command}: Program state does not change.", Command.Command.None);
        }

        private void Push(int codelBlockSize)
        {
            _logger.LogDebug(
                "Executing command {Command}: Push {CodelBlockSize} on the stack.", Command.Command.Push, codelBlockSize);
            _programStack.Push(codelBlockSize);
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = codelBlockSize;
            }
        }

        private void Pop()
        {
            if (_programStack.Count == 0)
            {
                _logger.LogDebug(
                    "Executing command {Command}: Program stack is empty, ignoring.", Command.Command.Pop);
                return;
            }

            _logger.LogDebug(
                "Executing command {Command}: Pop {StackTop} from the stack.", Command.Command.Pop, _programStack.Peek());
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = _programStack.Pop();
            }
            else
            {
                _programStack.Pop();
            }
        }

        private void Add()
        {
            if (_programStack.Count < 2)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Add);
                return;
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();
            var result = operandA + operandB;
            _programStack.Push(result);

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = result;
                _currentCommandInfo.OperandA = operandA;
                _currentCommandInfo.OperandB = operandB;
            }
        }

        private void Subtract()
        {
            if (_programStack.Count < 2)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Subtract);
                return;
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();
            var result = operandA - operandB;
            _programStack.Push(result);

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = result;
                _currentCommandInfo.OperandA = operandA;
                _currentCommandInfo.OperandB = operandB;
            }
        }

        private void Multiply()
        {
            if (_programStack.Count < 2)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Multiply);
                return;
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();
            var result = operandA * operandB;
            _programStack.Push(result);

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = result;
                _currentCommandInfo.OperandA = operandA;
                _currentCommandInfo.OperandB = operandB;
            }
        }

        private void Divide()
        {
            if (_programStack.Count < 2)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Divide);
                return;
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();

            if (operandB == 0)
            {
                // Division by zero: push operands back and ignore
                _programStack.Push(operandA);
                _programStack.Push(operandB);
                _logger.LogDebug("Executing command {Command}: Division by zero, ignoring.", Command.Command.Divide);
                return;
            }

            var result = operandA / operandB;
            _programStack.Push(result);

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = result;
                _currentCommandInfo.OperandA = operandA;
                _currentCommandInfo.OperandB = operandB;
            }
        }

        private void Modulo()
        {
            if (_programStack.Count < 2)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Modulo);
                return;
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();

            if (operandB == 0)
            {
                // Modulo by zero: push operands back and ignore
                _programStack.Push(operandA);
                _programStack.Push(operandB);
                _logger.LogDebug("Executing command {Command}: Modulo by zero, ignoring.", Command.Command.Modulo);
                return;
            }

            // Piet spec: result has the same sign as the divisor (operandB)
            var result = ((operandA % operandB) + operandB) % operandB;

            _programStack.Push(result);

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = result;
                _currentCommandInfo.OperandA = operandA;
                _currentCommandInfo.OperandB = operandB;
            }
        }

        private void Not()
        {
            if (_programStack.Count < 1)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Not);
                return;
            }

            var operand = _programStack.Pop();
            var result = operand == 0 ? 1 : 0;
            _programStack.Push(result);

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = result;
            }
        }

        private void GreaterThan()
        {
            if (_programStack.Count < 2)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Greater);
                return;
            }

            var operandB = _programStack.Pop();
            var operandA = _programStack.Pop();
            var result = operandA > operandB ? 1 : 0;
            _programStack.Push(result);

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = result;
                _currentCommandInfo.OperandA = operandA;
                _currentCommandInfo.OperandB = operandB;
            }
        }

        private void Pointer()
        {
            if (_programStack.Count < 1)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Pointer);
                return;
            }

            var operand = _programStack.Pop();
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = operand;
            }

            if (operand > 0)
            {
                for (int i = 0; i < operand % 4; i++)
                {
                    PietInterpreter.RotateDirectionPointerClockwise();
                }
            }
            else if (operand < 0)
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
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Switch);
                return;
            }

            var operand = _programStack.Pop();
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = operand;
            }

            if (Math.Abs(operand) % 2 == 1)
            {
                PietInterpreter.ToggleCodelChooser();
            }
        }

        private void Duplicate()
        {
            if (_programStack.Count < 1)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Duplicate);
                return;
            }

            var operand = _programStack.Peek();
            _programStack.Push(operand);
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = operand;
            }
        }

        private void Roll()
        {
            if (_programStack.Count < 2)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.Roll);
                return;
            }

            var numberOfRolls = _programStack.Pop();
            var depthOfRollOperation = _programStack.Pop();

            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.OperandA = numberOfRolls;
                _currentCommandInfo.OperandB = depthOfRollOperation;
            }

            if (depthOfRollOperation <= 0)
            {
                // Depth of 0 is a no-op; negative depth is ignored per spec
                return;
            }

            if (depthOfRollOperation > _programStack.Count)
            {
                _logger.LogDebug(
                    "Executing command {Command}: Depth {Depth} exceeds stack size {Size}, ignoring.",
                    Command.Command.Roll, depthOfRollOperation, _programStack.Count);
                return;
            }

            // Convert stack to array (bottom-to-top order) to perform roll operation
            var stackAsArray = _programStack.ToArray();
            Array.Reverse(stackAsArray);

            int rollInsertIndex = stackAsArray.Length - depthOfRollOperation;
            
            // Normalize number of rolls to avoid redundant cycles
            int normalizedRolls = numberOfRolls % depthOfRollOperation;

            if (normalizedRolls > 0)
            {
                // Positive roll: bury top element into the depth range
                for (int i = 0; i < normalizedRolls; i++)
                {
                    int topElement = stackAsArray[^1];
                    Array.Copy(stackAsArray, rollInsertIndex, stackAsArray, rollInsertIndex + 1,
                        stackAsArray.Length - rollInsertIndex - 1);
                    stackAsArray[rollInsertIndex] = topElement;
                }
            }
            else if (normalizedRolls < 0)
            {
                // Negative roll: dig bottom element of the depth range to top
                int absRolls = Math.Abs(normalizedRolls);
                for (int i = 0; i < absRolls; i++)
                {
                    int bottomElement = stackAsArray[rollInsertIndex];
                    Array.Copy(stackAsArray, rollInsertIndex + 1, stackAsArray, rollInsertIndex,
                        stackAsArray.Length - rollInsertIndex - 1);
                    stackAsArray[^1] = bottomElement;
                }
            }

            // Convert array back to stack
            _programStack.Clear();
            foreach (var number in stackAsArray)
            {
                _programStack.Push(number);
            }
        }

        private void InputNumberAsync(Context context)
        {
            InputService.RequestIntegerInputAsync();
            context.Pause?.Invoke();
        }

        private void InputCharacterAsync(Context context)
        {
            InputService.RequestCharacterInputAsync();
            context.Pause?.Invoke();
        }

        private void OutputNumber()
        {
            if (_programStack.Count < 1)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.OutputNumber);
                return;
            }

            var operand = _programStack.Pop();
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = operand;
            }
            _logger.LogDebug("Numeric output value {Operand}", operand);
            OutputService.DispatchOutputIntegerEvent(operand);
        }

        private void OutputCharacter()
        {
            if (_programStack.Count < 1)
            {
                _logger.LogDebug("Executing command {Command}: Insufficient elements on stack, ignoring.", Command.Command.OutputCharacter);
                return;
            }

            var operand = _programStack.Pop();
            if (_currentCommandInfo is not null)
            {
                _currentCommandInfo.Value = operand;
            }
            _logger.LogDebug("Character output value {Character}", Convert.ToChar(operand));
            OutputService.DispatchOutputCharacterEvent((char)operand);
        }
}