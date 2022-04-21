namespace Piet.Interpreter.Events;

public interface IOutputEventService
{
    event EventHandler<OutputCharacterOperationEventArgs>? OutputCharacter;
    event EventHandler<OutputIntegerOperationEventArgs>? OutputInteger;
    void DispatchOutputCharacterEvent(char value);
    void DispatchOutputIntegerEvent(int value);
}