using System.Collections.Immutable;
using Piet.Color;

namespace Piet.Command;


public static class PietCommandControl
{
    private const int HueLevels = 6;
    private const int SatuationLevels = 3;

    private static readonly ImmutableArray<ImmutableArray<Command>> _commandLookup = new()
    {
        new() {Command.None, Command.Add,      Command.Divide, Command.Greater, Command.Duplicate,   Command.InputCharacter},
        new() {Command.Push, Command.Subtract, Command.Modulo, Command.Pointer, Command.Roll,        Command.OutputNumber},
        new() {Command.Pop,  Command.Multiply, Command.Not,    Command.Switch,  Command.InputNumber, Command.OutputCharacter}
    };
    
    private static readonly ImmutableArray<ImmutableArray<PietColor>> _colorLookup = new()
    {
        new () {PietColors.LightRed, PietColors.LightYellow, PietColors.LightGreen, PietColors.LightCyan, PietColors.LightBlue, PietColors.LightMagenta},
        new () {PietColors.Red,      PietColors.Yellow,      PietColors.Green,      PietColors.Cyan,      PietColors.Blue,      PietColors.Magenta},
        new () {PietColors.DarkRed,  PietColors.DarkYellow,  PietColors.DarkGreen,  PietColors.DarkCyan,  PietColors.DarkBlue,  PietColors.DarkMagenta }
    };

    private static (int,int) GetIndicesOfCurrentColor(PietColor color)
    {
        for (int satuation = 0; satuation < SatuationLevels; satuation++)
        {
            for (int hue = 0; hue < HueLevels; hue++)
            {
                if (_colorLookup[satuation][hue] == color)
                {
                    return (satuation, hue);
                }
            }
        }

        throw new ArgumentException(
            $"PietColor ({color}) has no matching color in the lookup table");
    }

    internal static int IncrementHueIndex(int hueIndex)
    {
        return hueIndex++ < HueLevels-1 ? hueIndex : 0;
    }

    internal static int IncrementSatuationIndex(int satuationIndex)
    {
        return satuationIndex++ < SatuationLevels-1 ? satuationIndex : 0;
    }
    
    public static PietColorCommand[,] GetColorCommands(PietColor currentColor)
    {
        var (currentColorSatuationIndex, currentColorHueIndex) = GetIndicesOfCurrentColor(currentColor);

        var colorCommands = new PietColorCommand[HueLevels, SatuationLevels];

        for (int satuation = 0; satuation < SatuationLevels; satuation++)
        {
            for (int hue = 0; hue < HueLevels; hue++)
            {
                colorCommands[satuation, hue] = new PietColorCommand(_colorLookup[satuation][hue],
                    _commandLookup[currentColorSatuationIndex][currentColorHueIndex]);

                currentColorHueIndex = IncrementHueIndex(currentColorHueIndex);
            }
            currentColorSatuationIndex = IncrementHueIndex(currentColorSatuationIndex);
        }

        return colorCommands;
    }
}