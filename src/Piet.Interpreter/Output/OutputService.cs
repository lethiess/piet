using Piet.Interpreter.Exceptions;

namespace Piet.Interpreter.Output;

public class OutputService : IOutputService
{
    public event EventHandler<OutputCharacterOperationEventArgs>? OutputCharacter;
    public event EventHandler<OutputIntegerOperationEventArgs>? OutputInteger;
    public event EventHandler<OutputCommandLogEventArg>? OutputCommandLog;
    public event EventHandler<InterpreterExceptionEventArgs>? OutputException;

    public void DispatchOutputCharacterEvent(char value)
    {
        OnOutputCharacterOperation(new OutputCharacterOperationEventArgs(value));
    }

    public void DispatchOutputIntegerEvent(int value)
    {
        OnOutputIntegerOperation(new OutputIntegerOperationEventArgs(value));
    }

    public void DispatchOutputCommandLogEvent(CommandInfo commandInfo)
    {
        OnOutputCommandLogOperation(new OutputCommandLogEventArg(commandInfo));
    }

    public void DispatchOutputExceptionEvent(InterpreterExceptionBase exception)
    {
        OnOutputExceptionOperation(new InterpreterExceptionEventArgs(exception.Message));
    }

    protected virtual void OnOutputCharacterOperation(OutputCharacterOperationEventArgs e)
    {
        var handler = OutputCharacter;
        handler?.Invoke(this, e);
    }
        
    protected virtual void OnOutputIntegerOperation(OutputIntegerOperationEventArgs e)
    {
        var handler = OutputInteger;
        handler?.Invoke(this, e);
    }

    protected virtual void OnOutputCommandLogOperation(OutputCommandLogEventArg e)
    {
        var handler = OutputCommandLog;
        handler?.Invoke(this, e);
    }

    protected virtual void OnOutputExceptionOperation(InterpreterExceptionEventArgs e)
    {
        var handler = OutputException;
        handler?.Invoke(this, e);
    }
}