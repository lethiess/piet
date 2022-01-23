using System.Collections.Immutable;

namespace Piet.Color
{
    internal static class PietColorDefinitions
    {
        private static readonly ImmutableList<(int R, int G, int B)> _validPietColor = new List<(int R, int G, int B)>
        {
            (1, 2, 3),

        }.ToImmutableList();

        public static bool IsValid(int red, int green, int blue)
        {
            return _validPietColor.Contains((red, green, blue));
        }
    }
}