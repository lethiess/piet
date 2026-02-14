---
name: piet-interpreter
description: Develop and maintain the Piet esoteric language interpreter and Blazor web UI. Use when working on interpreter logic, command execution, codel navigation, color mapping, or the web UI for this Piet project.
---

# Piet Interpreter Development

## Project Architecture

```
src/
├── Piet.Color/          # Color types (PietColor, PietColors constants)
├── Piet.Command/        # ColorCommandControl, command enum, color-to-command mapping
├── Piet.CodelGrid/      # Codel, CodelGrid, CodelGridBuilder
├── Piet.Interpreter/    # Core interpreter engine
│   ├── PietInterpreter.cs      # Main execution loop, state management
│   ├── ProgramOperator.cs      # Stack operations (push, pop, add, roll, etc.)
│   ├── CodelChooser.cs         # Edge selection, transition codel, white sliding
│   ├── CodelBlockSearcher.cs   # Region growing for connected codel blocks
│   └── PietInterpreterServiceExtension.cs  # DI registration
└── Piet.Web/            # Blazor WASM UI
    └── Pages/PietProgram.razor  # Main grid editor and interpreter UI
```

## Piet Language Quick Reference

Spec: https://www.dangermouse.net/esoteric/piet.html

### Execution Model
1. Find current codel block (region growing from current codel)
2. Find edge of block furthest in DP direction
3. Pick transition codel from edge using CC (left/right of DP direction)
4. Move one step in DP direction from transition codel
5. If blocked: toggle CC, try again; if still blocked: rotate DP, repeat (8 attempts total)
6. If 8 attempts fail: program terminates

### Command Mapping
Commands are determined by hue change (0-5) x saturation change (0-2) between current and next color block.

### Critical Rules (bugs found and fixed in this codebase)
- **Stack underflow**: silently ignore the operation (do NOT throw)
- **Modulo**: result sign matches the divisor: `((a % b) + b) % b`
- **Roll**: negative rolls go in reverse direction; depth 0 is no-op
- **White codels**: have their own 4-attempt slide retry, separate from the main 8-attempt loop
- **Division/modulo by zero**: push operands back and ignore

## Testing Patterns

Tests are in `Piet.Interpreter.UnitTests/` and `Piet.Command.UnitTets/`.

```csharp
// Standard test setup for ProgramOperator
var outputMock = new Mock<IOutputService>();
var inputMock = new Mock<IInputService>();
var op = new ProgramOperator(new NullLogger<ProgramOperator>(),
    outputMock.Object, inputMock.Object);

// Push values onto stack
op.ExecuteCommand(new ColorCommand(PietColors.Green, Command.Command.Push),
    GetFakeCodelBlockOfSize(value), new Context());

// Verify stack state
var stack = op.GetProgramStack(); // returns List<int>, top-first
```

For CodelChooser tests, set static state before each test:
```csharp
PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;
```

## Known Limitations
- `DirectionPointer` and `CodelChooserState` are static mutable state on `PietInterpreter` -- acceptable for single-threaded Blazor WASM but not thread-safe
- The `Codel` record has a mutable `Color` property for grid editing support
