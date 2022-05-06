namespace Piet.Interpreter.Input;

class InputRequestService : IInputRequestService
{
    public event EventHandler<EventArgs>? CharacterRequest;
    public event EventHandler<EventArgs>? IntegerRequest;

    public void SendIntegerInputRequest()
    {
        OnInputIntegerOperation();
    }

    public void SendCharacterInputRequest()
    {
        OnInputCharacterOperation();
    }

    protected virtual void OnInputCharacterOperation()
    {
        var handler = CharacterRequest;
        handler?.Invoke(this, new EventArgs());
    }

    protected virtual void OnInputIntegerOperation()
    {
        var handler = IntegerRequest;
        handler?.Invoke(this, new EventArgs());
    }
}