using Microsoft.Extensions.DependencyInjection;
using Piet.Grid;
using Piet.Interpreter.Events;

namespace Piet.Interpreter;

public static class ServiceExtension
{
    public static IServiceCollection AddPietInterpreter(
        this IServiceCollection services
    )
    {
        services.AddTransient<ICodelChooser, CodelChooser>();
        services.AddTransient<ICodelBlockSearcher, CodelBlockSearcher>();
        services.AddSingleton<IOutputEventService, OutputEventService>();
        return services;
    }
}