namespace Piet.Interpreter.Input;

internal class InputResponseService : IInputResponseService
{
    public event EventHandler<InputCharacterResponseEventArgs>? InputCharacterResponse;
    public event EventHandler<InputIntegerResponseEventArgs>? InputIntegerResponse;

    private int? _integerInput;
    private char? _charInput;
    public InputResponseService()
    {
        InputIntegerResponse += OnInputIntegerResponse;
        InputCharacterResponse += OnInputCharacterResponse;
    }

    public Task<int> GetIntegerResult()
    {
        //while (_integerInput is null)
        //{
        //    Thread.Sleep(10);
        //}

        //return Task.FromResult((int)_integerInput);
        return Task.FromResult(12);
    }

    public Task<char> GetCharacterResult()
    {
        while (_charInput is null)
        {
            Thread.Sleep(10);
        }

        return Task.FromResult((char)_charInput);
    }

    public void SendInputIntegerResponse(int value)
    {
        OnSendInputIntegerResponse(new InputIntegerResponseEventArgs(value));
    }
    public void SendInputCharacterResponse(char value)
    {
        OnSendInputCharacterResponse(new InputCharacterResponseEventArgs(value));
    }

    public virtual void OnSendInputIntegerResponse(InputIntegerResponseEventArgs e)
    {
        var handler = InputIntegerResponse;
        handler?.Invoke(this, e);
    }

    public virtual void OnSendInputCharacterResponse(InputCharacterResponseEventArgs e)
    {
        var handler = InputCharacterResponse;
        handler?.Invoke(this, e);
    }

    protected virtual void OnInputIntegerResponse(object? sender,
        InputIntegerResponseEventArgs e)
    {
        _integerInput = e.Value;
    }

    protected virtual void OnInputCharacterResponse(object? sender,
        InputCharacterResponseEventArgs e)
    {
        _charInput = e.Value;
    }
}