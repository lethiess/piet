using Microsoft.Extensions.DependencyInjection;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter;

public static class PietInterpreterServiceExtension
{
    public static IServiceCollection AddPietInterpreter(
        this IServiceCollection services
    )
    {
        services.AddScoped<ICodelChooser, CodelChooser>();
        services.AddScoped<ICodelBlockSearcher, CodelBlockSearcher>();
        services.AddScoped<IProgramOperator, ProgramOperator>();
        services.AddScoped<IInputService, InputService>();
        services.AddScoped<IOutputService, OutputService>();
        return services;
    }
}