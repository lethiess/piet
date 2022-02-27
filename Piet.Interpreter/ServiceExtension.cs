using Microsoft.Extensions.DependencyInjection;

namespace Piet.Interpreter;

public static class ServiceExtension
{
    public static IServiceCollection AddPietInterpreter(
        this IServiceCollection services
    )
    {
        services.AddTransient<PietInterpreter>();
        services.AddTransient<DirectionPointer>();
        services.AddTransient<CodelChooser>();
        return services;
    }
}