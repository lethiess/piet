namespace Piet.Interpreter.Output;

public interface IOutputService
{
    event EventHandler<OutputCharacterOperationEventArgs>? OutputCharacter;
    event EventHandler<OutputIntegerOperationEventArgs>? OutputInteger;
    event EventHandler<OutputCommandLogEventArg>? OutputCommandLog; 
    void DispatchOutputCharacterEvent(char value);
    void DispatchOutputIntegerEvent(int value);
    void DispatchOutputCommandLogEvent(CommandInfo commandInfo);

}