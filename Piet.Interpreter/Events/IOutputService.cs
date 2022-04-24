namespace Piet.Interpreter.Events;

public interface IOutputService
{
    event EventHandler<OutputCharacterOperationEventArgs>? OutputCharacter;
    event EventHandler<OutputIntegerOperationEventArgs>? OutputInteger;
    void DispatchOutputCharacterEvent(char value);
    void DispatchOutputIntegerEvent(int value);
}