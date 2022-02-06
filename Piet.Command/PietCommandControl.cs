using System.Diagnostics;
using Piet.Color;

namespace Piet.Command;


// input: cell color (current)
// output: command behind all colors based in input



public static class PietCommandControl
{
    private const int Hue = 6;
    private const int Satuation = 3;

    private static readonly Command[,] _commands = new Command[,]
    {
        { Command.None, Command.Push, Command.Pop },
        { Command.Add, Command.Subtract, Command.Multiply},
        { Command.Divide, Command.Modulo, Command.Not},
        { Command.Greater, Command.Pointer, Command.Switch},
        { Command.Duplicate, Command.Roll, Command.InputNumber},
        { Command.InputCharacter, Command.OuputNumber, Command.OutputCharacter}
    };

    private static readonly PietColor[,] _colors = new PietColor[,]
    {
        {PietColors.LightRed, PietColors.LightYellow, PietColors.LightGreen, PietColors.LightCyan, PietColors.LightBlue, PietColors.LightMagenta},
        {PietColors.Red,      PietColors.Yellow,      PietColors.Green,      PietColors.Cyan,      PietColors.Blue,      PietColors.Magenta},
        {PietColors.DarkRed,  PietColors.DarkYellow,  PietColors.DarkGreen,  PietColors.DarkCyan,  PietColors.DarkBlue,  PietColors.DarkMagenta }
    };

    private static (int,int) GetIndicesOfCurrentColor(PietColor color)
    {
        for (int satuation = 0; satuation < Satuation; satuation++)
        {
            for (int hue = 0; hue < Hue; hue++)
            {
                if (_colors[satuation,hue] == color)
                {
                    return (satuation, hue);
                }
            }
        }

        throw new ArgumentException(
            $"PietColor ({color}) has no matching color in the lookup table");
    }

    private static Command GetColorCommand(
        int satuation, 
        int hue, 
        int satuationOffset,
        int hueOffset)
    {

        // todo: return corresponding to current colors offset

        throw new NotImplementedException();
    }

    public static PietColorCommand[,] GetColorCommands(PietColor currentColor)
    {
        var (satuationIndex, hueIndex) = GetIndicesOfCurrentColor(currentColor);

        var colorCommands = new PietColorCommand[Hue, Satuation];

        for (int satuation = 0; satuation < Hue; satuation++)
        {
            for (int hue = 0; hue < Hue; hue++)
            {
                colorCommands[satuation, hue] = new PietColorCommand(_colors[satuation, hue],
                    GetColorCommand(satuation, hue, satuationIndex, hueIndex));
            }
        }

        return colorCommands;
    }
}


public record PietColorCommand(
   PietColor Color,
   Command Command)
{
}

public enum Command
{
    None,
    Push, 
    Pop,
    Add, 
    Subtract,
    Multiply,
    Divide,
    Modulo,
    Not,
    Greater,
    Pointer,
    Switch,
    Duplicate,
    Roll, 
    InputNumber,
    InputCharacter,
    OuputNumber,
    OutputCharacter
}