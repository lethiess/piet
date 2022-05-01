﻿using Microsoft.Extensions.DependencyInjection;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter;

public static class PietInterpreterServiceExtension
{
    public static IServiceCollection AddPietInterpreter(
        this IServiceCollection services
    )
    {
        services.AddTransient<IProgramOperator, ProgramOperator>();
        services.AddTransient<IInputResponseService, InputResponseService>();
        services.AddTransient<IInputRequestService, InputRequestService>();
        services.AddTransient<IInputFacade, InputFacade>();
        services.AddTransient<IOutputService, OutputService>();
        return services;
    }
}