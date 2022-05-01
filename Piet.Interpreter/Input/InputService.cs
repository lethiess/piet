namespace Piet.Interpreter.Input;

class InputService : IInputService
{
    public event EventHandler<EventArgs>? InputCharacter;
    public event EventHandler<EventArgs>? InputInteger;

    public async Task<int> GetIntegerInputAsync()
    {
        OnInputIntegerOperation(new EventArgs());


        throw new NotImplementedException();
    }

    public async Task<char> GetCharacterInputAsync()
    {
        OnInputIntegerOperation(new EventArgs());



        throw new NotImplementedException();
    }

    protected virtual void OnInputCharacterOperation(EventArgs e)
    {
        var handler = InputCharacter;
        handler?.Invoke(this, e);
    }

    protected virtual void OnInputIntegerOperation(EventArgs e)
    {
        var handler = InputInteger;
        handler?.Invoke(this, e);
    }
}