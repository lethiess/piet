using System.Collections.Immutable;
using Piet.Color;

namespace Piet.Command;


public static class PietCommandControl
{
    private const int HueLevels = 6;
    private const int SatuationLevels = 3;

    private static readonly ImmutableArray<ImmutableArray<Command>> _commandLookup =
        ImmutableArray.Create<ImmutableArray<Command>>(
            ImmutableArray.Create<Command>(Command.None, Command.Add,      Command.Divide, Command.Greater, Command.Duplicate,   Command.InputCharacter),
            ImmutableArray.Create<Command>(Command.Push, Command.Subtract, Command.Modulo, Command.Pointer, Command.Roll,        Command.OutputNumber),
            ImmutableArray.Create<Command>(Command.Pop,  Command.Multiply, Command.Not,    Command.Switch,  Command.InputNumber, Command.OutputCharacter)
    );

    private static readonly ImmutableArray<ImmutableArray<PietColor>> _colorLookup =
        ImmutableArray.Create<ImmutableArray<PietColor>>(
            ImmutableArray.Create<PietColor>(PietColors.LightRed, PietColors.LightYellow, PietColors.LightGreen, PietColors.LightCyan, PietColors.LightBlue, PietColors.LightMagenta),
            ImmutableArray.Create<PietColor>(PietColors.Red,      PietColors.Yellow,      PietColors.Green,      PietColors.Cyan,      PietColors.Blue,      PietColors.Magenta),
            ImmutableArray.Create<PietColor>(PietColors.DarkRed,  PietColors.DarkYellow,  PietColors.DarkGreen,  PietColors.DarkCyan,  PietColors.DarkBlue,  PietColors.DarkMagenta )
    );

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

    private static Command GetCommand(int satuarionIndex, int hueIndex,
        int currentColorSatuationIndex, int currentColorHueIndex)
    {

        var x = hueIndex - currentColorHueIndex;
        var commandIndexX = x >= 0 ? x : HueLevels - Math.Abs(x);
        var y = satuarionIndex - currentColorSatuationIndex;
        var commandIndexY = y >= 0 ? y: SatuationLevels - Math.Abs(y);

        return _commandLookup[commandIndexY][commandIndexX];
    }
    
    public static PietColorCommand[,] GetColorCommands(PietColor currentColor)
    {
        var (currentColorSatuationIndex, currentColorHueIndex) = GetIndicesOfCurrentColor(currentColor);

        var colorCommands = new PietColorCommand[SatuationLevels, HueLevels];

        for (int satuation = 0; satuation < SatuationLevels; satuation++)
        {
            for (int hue = 0; hue < HueLevels; hue++)
            {
                colorCommands[satuation, hue] = new PietColorCommand(_colorLookup[satuation][hue],
                    GetCommand(satuation, hue, currentColorSatuationIndex, currentColorHueIndex));

                //currentColorHueIndex = IncrementHueIndex(currentColorHueIndex);
            }
            //currentColorSatuationIndex = IncrementSatuationIndex(currentColorSatuationIndex);
        }

        return colorCommands;
    }
}