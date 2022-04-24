namespace Piet.Interpreter;

public interface IInputService
{
    Task<int> GetIntegerInput();
    Task<char> GetCharacterInput();
}