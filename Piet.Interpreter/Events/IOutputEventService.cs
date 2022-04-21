namespace Piet.Interpreter.Events;

public interface IOutputEventService
{
    void DispatchOutputCharacterEvent(char value);
    void DispatchOutputIntegerEvent(int value);
}