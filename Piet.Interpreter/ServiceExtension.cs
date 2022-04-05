using Microsoft.Extensions.DependencyInjection;
using Piet.Grid;

namespace Piet.Interpreter;

public static class ServiceExtension
{
    public static IServiceCollection AddPietInterpreter(
        this IServiceCollection services
    )
    {
        services.AddTransient<PietInterpreter>();
        services.AddScoped<ICodelGrid, CodelGrid>();
        services.AddTransient<ICodelChooser, CodelChooser>();
        services.AddTransient<ICodelBlockSearcher, CodelBlockSearcher>();
        return services;
    }
}