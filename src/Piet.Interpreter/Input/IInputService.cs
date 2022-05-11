namespace Piet.Interpreter.Input;

public interface IInputService
{
    event EventHandler<EventArgs>? CharacterRequest;
    event EventHandler<EventArgs>? IntegerRequest;
    void RequestIntegerInputAsync();
    void RequestCharacterInputAsync();
}