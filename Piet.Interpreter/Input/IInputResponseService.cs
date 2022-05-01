namespace Piet.Interpreter.Input;

public interface IInputResponseService
{
    event EventHandler<InputCharacterResponseEventArgs>? InputCharacterResponse;
    event EventHandler<InputIntegerResponseEventArgs>? InputIntegerResponse;
    void SendInputIntegerResponse(int value);
    void SendInputCharacterResponse(char value);
    Task<int> GetIntegerResult();
    Task<char> GetCharacterResult();
}