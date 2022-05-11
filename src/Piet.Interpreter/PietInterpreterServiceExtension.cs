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
        services.AddTransient<ICodelChooser, CodelChooser>();
        services.AddTransient<ICodelBlockSearcher, CodelBlockSearcher>();
        services.AddTransient<IProgramOperator, ProgramOperator>();
        services.AddTransient<IInputService, InputService>();
        services.AddTransient<IOutputService, OutputService>();
        return services;
    }
}