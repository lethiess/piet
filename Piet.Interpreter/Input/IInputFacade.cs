namespace Piet.Interpreter.Input;

public interface IInputFacade
{
    IInputRequestService InputRequestService { get; }
    IInputResponseService InputResponseService { get; }
    Task<int> GetIntegerInputAsync();
    Task<char> GetCharacterInputAsync();
}