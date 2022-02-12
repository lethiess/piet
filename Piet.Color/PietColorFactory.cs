using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Piet.Color;

public static class PietColorFactory
{
    private static readonly
        ImmutableDictionary<PietColorNames, (int red, int green, int blue)>
        s_pietColorMapping =
            new Dictionary<PietColorNames, (int red, int green, int blue)>
            {
                {
                    PietColorNames.LightRed, (0xFF, 0xC0, 0xC0)
                },
                {
                    PietColorNames.Red, (0xFF, 0x00, 0x00)
                },
                {
                    PietColorNames.DarkRed, (0xC0, 0x00, 0x00)
                },
                {
                    PietColorNames.LightYellow, (0xFF, 0xFF, 0xC0)
                },
                {
                    PietColorNames.Yellow, (0xFF, 0xFF, 0x00)
                },
                {
                    PietColorNames.DarkYellow, (0xC0, 0xC0, 0x00)
                },
                {
                    PietColorNames.LightGreen, (0xC0, 0xFF, 0xC0)
                },
                {
                    PietColorNames.Green, (0x00, 0xFF, 0x00)
                },
                {
                    PietColorNames.DarkGreen, (0x00, 0xC0, 0x00)
                },
                {
                    PietColorNames.LightCyan, (0xC0, 0xFF, 0xFF)
                },
                {
                    PietColorNames.Cyan, (0x00, 0xFF, 0xFF)
                },
                {
                    PietColorNames.DarkCyan, (0x00, 0xC0, 0xC0)
                },
                {
                    PietColorNames.LightBlue, (0xC0, 0xC0, 0xFF)
                },
                {
                    PietColorNames.Blue, (0x00, 0x00, 0xFF)
                },
                {
                    PietColorNames.DarkBlue, (0x00, 0x00, 0xC0)
                },
                {
                    PietColorNames.LightMagenta, (0xFF, 0xC0, 0xFF)
                },
                {
                    PietColorNames.Magenta, (0xFF, 0x00, 0xFF)
                },
                {
                    PietColorNames.DarkMagenta, (0xC0, 0x00, 0xC0)
                },
                {
                    PietColorNames.Black, (0x00, 0x00, 0x00)
                },
                {
                    PietColorNames.White, (0xFF, 0xFF, 0xFF)
                }

            }.ToImmutableDictionary();

    public static PietColor Create(PietColorNames pietColorName)
    {
        var (red, green, blue) = s_pietColorMapping.GetValueOrDefault(pietColorName);
        return new PietColor(red, green, blue, pietColorName);
    }

    public static PietColor Create(int red, int green, int blue)
    {
        if (s_pietColorMapping.ContainsValue((red, green, blue)) is false)
        {
            throw new InvalidPietColorCodeException($"Color code R:{red} G:{green} B:{blue} is a invalid Piet color");
        }

        var color = s_pietColorMapping.Single(x => x.Value == (red, green, blue));

        return new PietColor(red, green, blue, color.Key);
    }

    public static PietColor CreateRandomColor()
    {
        var random = new Random();
        var pietColorNameList = Enum.GetValues<PietColorNames>();
        var colorName =
            (PietColorNames)(pietColorNameList.GetValue(
                                 random.Next(pietColorNameList.Length)) ??
                             throw new InvalidOperationException($"The enum {typeof(PietColorNames)} contains no elements"));

        return Create(colorName);
    }
}