using Microsoft.Extensions.DependencyInjection;
using Piet.Interpreter.Events;

namespace Piet.Interpreter;

public static class PietInterpreterServiceExtension
{
    public static IServiceCollection AddPietInterpreter(
        this IServiceCollection services
    )
    {
        services.AddTransient<IProgramOperator, ProgramOperator>();
        services.AddSingleton<IInputService, InputService>();
        services.AddSingleton<IOutputService, OutputService>();
        return services;
    }
}