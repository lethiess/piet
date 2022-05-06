namespace Piet.Interpreter.Input;

public interface IInputRequestService
{
    event EventHandler<EventArgs>? CharacterRequest;
    event EventHandler<EventArgs>? IntegerRequest;
    void SendIntegerInputRequest();
    void SendCharacterInputRequest();

}