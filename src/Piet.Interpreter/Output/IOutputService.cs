using Piet.Interpreter.Exceptions;

namespace Piet.Interpreter.Output;

public interface IOutputService
{
    event EventHandler<OutputCharacterOperationEventArgs>? OutputCharacter;
    event EventHandler<OutputIntegerOperationEventArgs>? OutputInteger;
    event EventHandler<OutputCommandLogEventArg>? OutputCommandLog;
    event EventHandler<InterpreterExceptionEventArgs>? InterpreterException;
    event EventHandler<ProgramOperatorUpdateEventArgs>? ProgramOperatorUpdate;
    void DispatchOutputCharacterEvent(char value);
    void DispatchOutputIntegerEvent(int value);
    void DispatchOutputCommandLogEvent(CommandInfo commandInfo);
    void DispatchOutputExceptionEvent(InterpreterExceptionBase exception);
    void DispatchOutputProgramOperatorUpdateEvent(Stack<int> currentStack);

}