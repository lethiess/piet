using System.Collections.Generic;
using Piet.Color;
using Xunit;

namespace Piet.Command.UnitTests;

public partial class CommandControlUnitTests
{
    public static IEnumerable<object[]> GetColorCommandTestData()
    {
        yield return new object[]
        {
            PietColors.LightRed,
            PietColors.LightYellow,
            Command.Add
        };
        
        yield return new object[]
        {
            PietColors.Yellow,
            PietColors.LightMagenta,
            Command.InputNumber
        };
        
        yield return new object[]
        {
            PietColors.DarkGreen,
            PietColors.Red,
            Command.InputNumber
        };
        
        yield return new object[]
        {
            PietColors.LightBlue,
            PietColors.DarkCyan,
            Command.OutputCharacter
        };
        
        yield return new object[]
        {
            PietColors.Red,
            PietColors.Black,
            Command.None
        };
        
        yield return new object[]
        {
            PietColors.Red,
            PietColors.White,
            Command.None
        };
    }
    
    [Theory]
    [MemberData(nameof(GetColorCommandTestData))]
    public void GetCommand(PietColor currentColor, PietColor nextColor, Command expectedCommand)
    {
        var colorCommand = ColorCommandControl.GetColorCommand(currentColor, nextColor);
        Assert.Equal(expectedCommand, colorCommand.Command);
    }
}