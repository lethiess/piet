namespace Piet.Interpreter.Input;

class InputFacade : IInputFacade
{
    public InputFacade(
        IInputRequestService inputRequestService,
        IInputResponseService inputResponseService)
    {
        InputRequestService = inputRequestService;
        InputResponseService = inputResponseService;
    }

    public IInputRequestService InputRequestService { get; }
    public IInputResponseService InputResponseService { get; }

    public async Task<int> GetIntegerInputAsync()
    {
        InputRequestService.SendIntegerInputRequest();
        return await InputResponseService.GetIntegerResult();
    }

    public async Task<char> GetCharacterInputAsync()
    {
        InputRequestService.SendCharacterInputRequest();
        return await InputResponseService.GetCharacterResult();
    }
}