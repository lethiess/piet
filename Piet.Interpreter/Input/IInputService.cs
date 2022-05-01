namespace Piet.Interpreter.Input;

public interface IInputService
{
    event EventHandler<EventArgs>? InputCharacter;
    event EventHandler<EventArgs>? InputInteger;
    Task<int> GetIntegerInputAsync();
    Task<char> GetCharacterInputAsync();
}