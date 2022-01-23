namespace Piet.Color;

public static class PietColorExtensions
{
    public static PietColor GetRandomColor(this PietColor me)
    {
        Random random = new Random();
        var pietColorNameList = Enum.GetValues<PietColorNames>();
        var colorName =
            (PietColorNames)(pietColorNameList.GetValue(
                                 random.Next(pietColorNameList.Length)) ??
                             throw new InvalidOperationException($"The enum {typeof(PietColorNames)} contains no elements"));

        me = new PietColor(colorName);

        return me;
    }

}