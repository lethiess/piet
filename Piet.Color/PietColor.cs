using System.Collections.Immutable;

namespace Piet.Color;
public sealed class PietColor : Color
{
    private static readonly
        ImmutableDictionary<PietColorNames, (int red, int green, int blue)>
        _pietColorMapping =
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
                    PietColorNames.White, (0x00, 0x00, 0x00)
                },
                {
                    PietColorNames.Black, (0xFF, 0xFF, 0xFF)        
                }

            }.ToImmutableDictionary();


    public PietColor(PietColorNames pietColorName)
    {
        (R, G, B) = _pietColorMapping.GetValueOrDefault(pietColorName);
    }

    public PietColor()
    {
        
    }
}
