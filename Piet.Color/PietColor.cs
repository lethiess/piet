namespace Piet.Color
{
    internal sealed class PietColor : Color
    {
        public PietColor(PietColorNames colorName)
        {
            var color = GetColor(colorName);
        }
    }
}