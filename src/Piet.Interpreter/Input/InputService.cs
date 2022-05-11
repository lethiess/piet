namespace Piet.Interpreter.Input;

class InputService : IInputService
{

    public event EventHandler<EventArgs>? CharacterRequest;
    public event EventHandler<EventArgs>? IntegerRequest;
    public void RequestIntegerInputAsync()
    {
        OnInputIntegerOperation();
    }

    public void RequestCharacterInputAsync()
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