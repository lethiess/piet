using System;
using System.Collections.Immutable;
using Piet.Color;

namespace Piet.Command;

public static class ColorCommandControl
{
    public const int HueLevels = 6;
    public const int SaturationLevels = 3;

    private static readonly ImmutableArray<ImmutableArray<Command>> _commandLookup =
        ImmutableArray.Create(
            ImmutableArray.Create(Command.None, Command.Add,      Command.Divide, Command.Greater, Command.Duplicate,   Command.InputCharacter),
            ImmutableArray.Create(Command.Push, Command.Subtract, Command.Modulo, Command.Pointer, Command.Roll,        Command.OutputNumber),
            ImmutableArray.Create(Command.Pop,  Command.Multiply, Command.Not,    Command.Switch,  Command.InputNumber, Command.OutputCharacter)
    );

    private static readonly ImmutableArray<ImmutableArray<PietColor>> _colorLookup =
        ImmutableArray.Create(
            ImmutableArray.Create(PietColors.LightRed, PietColors.LightYellow, PietColors.LightGreen, PietColors.LightCyan, PietColors.LightBlue, PietColors.LightMagenta),
            ImmutableArray.Create(PietColors.Red,      PietColors.Yellow,      PietColors.Green,      PietColors.Cyan,      PietColors.Blue,      PietColors.Magenta),
            ImmutableArray.Create(PietColors.DarkRed,  PietColors.DarkYellow,  PietColors.DarkGreen,  PietColors.DarkCyan,  PietColors.DarkBlue,  PietColors.DarkMagenta )
    );

    private static (int,int) GetIndicesOfCurrentColor(PietColor color)
    {
        for (int saturation = 0; saturation < SaturationLevels; saturation++)
        {
            for (int hue = 0; hue < HueLevels; hue++)
            {
                if (_colorLookup[saturation][hue] == color)
                {
                    return (saturation, hue);
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

    private static int GetSaturationIndexOffset(int saturationIndex, int currentColorSaturationIndex)
    {
        var offset = saturationIndex - currentColorSaturationIndex;
        return offset >= 0 ? offset : SaturationLevels - Math.Abs(offset);
    }

    private static Command GetCommand(int saturationIndex, int hueIndex,
        int currentColorSaturationIndex, int currentColorHueIndex)
    {
        return _commandLookup[GetSaturationIndexOffset(saturationIndex, currentColorSaturationIndex)][
            GetHueIndexOffset(hueIndex, currentColorHueIndex)];
    }
    
    public static ColorCommand[,] GetColorCommands(PietColor currentColor)
    {
        var (currentColorSaturationIndex, currentColorHueIndex) = GetIndicesOfCurrentColor(currentColor);

        var colorCommands = new ColorCommand[SaturationLevels, HueLevels];

        for (int saturationIndex = 0; saturationIndex < SaturationLevels; saturationIndex++)
        {
            for (int hueIndex = 0; hueIndex < HueLevels; hueIndex++)
            {
                colorCommands[saturationIndex, hueIndex] = new ColorCommand(
                    _colorLookup[saturationIndex][hueIndex],
                    GetCommand(saturationIndex, hueIndex, currentColorSaturationIndex,
                        currentColorHueIndex));
            }
        }

        return colorCommands;
    }

    public static ColorCommand GetColorCommand(PietColor currentColor, PietColor nextColor)
    {
        if (nextColor == PietColors.Black || nextColor == PietColors.White)
        {
            return new ColorCommand(nextColor, Command.None);
        }
        var currentColorCommands = GetColorCommands(currentColor);
        var (nextColorIndexX, nextColorIndexY) = GetIndicesOfCurrentColor(nextColor);
        
        return currentColorCommands[nextColorIndexX, nextColorIndexY];
    }
}