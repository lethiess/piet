﻿using System.Collections.Immutable;
using Piet.Command;
using Piet.Grid;
using Piet.Interpreter.Input;
using Piet.Interpreter.Output;

namespace Piet.Interpreter;

public interface IProgramOperator
{
    IInputService InputService { get; }
    IOutputService OutputService { get; }
    void SetInputValue(int input, ColorCommand colorCommand);
    void Reset();
    void ExecuteCommand(ColorCommand colorCommand, ImmutableList<Codel> codelBlock, Context context);
}