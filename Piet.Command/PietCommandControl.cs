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

    private static int GetHueIndexOffset(int hueIndex, int currentColorHueIndex)
    {
        var offset = hueIndex - currentColorHueIndex;
        return offset >= 0 ? offset : HueLevels - Math.Abs(offset);
    }

    private static int GetSatuationIndexOffset(int satuationIndex, int currentColorSatuationIndex)
    {
        var offset = satuationIndex - currentColorSatuationIndex;
        return offset >= 0 ? offset : SatuationLevels - Math.Abs(offset);
    }

    private static Command GetCommand(int satuationIndex, int hueIndex,
        int currentColorSatuationIndex, int currentColorHueIndex)
    {
        return _commandLookup[GetSatuationIndexOffset(satuationIndex, currentColorSatuationIndex)][
            GetHueIndexOffset(hueIndex, currentColorHueIndex)];
    }
    
    public static PietColorCommand[,] GetColorCommands(PietColor currentColor)
    {
        var (currentColorSatuationIndex, currentColorHueIndex) = GetIndicesOfCurrentColor(currentColor);

        var colorCommands = new PietColorCommand[SatuationLevels, HueLevels];

        for (int satuationIndex = 0; satuationIndex < SatuationLevels; satuationIndex++)
        {
            for (int hueIndex = 0; hueIndex < HueLevels; hueIndex++)
            {
                colorCommands[satuationIndex, hueIndex] = new PietColorCommand(
                    _colorLookup[satuationIndex][hueIndex],
                    GetCommand(satuationIndex, hueIndex, currentColorSatuationIndex,
                        currentColorHueIndex));
            }
        }

        return colorCommands;
    }
}