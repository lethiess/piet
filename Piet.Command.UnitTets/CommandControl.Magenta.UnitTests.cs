using Piet.Color;
using Xunit;

namespace Piet.Command.UnitTests;

public partial class CommandControlUnitTests
{
    [Fact]
    public void GetColorCommands_ValidInput_ColorMagenta_MustReturn_NotNull()
    {
        var colorCommands = ColorCommandControl.GetColorCommands(PietColors.Magenta);
        Assert.NotNull(colorCommands);
    }

    [Fact]
    public void GetColorCommands_ValidInput_ColorMagenta_MustReturn_MustMatch()
    {
        var colorCommands = ColorCommandControl.GetColorCommands(PietColors.Magenta);
        Assert.NotNull(colorCommands);

        // validate commands
        Assert.Equal(Command.Multiply, colorCommands[0, 0].Command);
        Assert.Equal(Command.Not, colorCommands[0, 1].Command);
        Assert.Equal(Command.Switch, colorCommands[0, 2].Command);
        Assert.Equal(Command.InputNumber, colorCommands[0, 3].Command);
        Assert.Equal(Command.OutputCharacter, colorCommands[0, 4].Command);
        Assert.Equal(Command.Pop, colorCommands[0, 5].Command);

        Assert.Equal(Command.Add, colorCommands[1, 0].Command);
        Assert.Equal(Command.Divide, colorCommands[1, 1].Command);
        Assert.Equal(Command.Greater, colorCommands[1, 2].Command);
        Assert.Equal(Command.Duplicate, colorCommands[1, 3].Command);
        Assert.Equal(Command.InputCharacter, colorCommands[1, 4].Command);
        Assert.Equal(Command.None, colorCommands[1, 5].Command);

        Assert.Equal(Command.Subtract, colorCommands[2, 0].Command);
        Assert.Equal(Command.Modulo, colorCommands[2, 1].Command);
        Assert.Equal(Command.Pointer, colorCommands[2, 2].Command);
        Assert.Equal(Command.Roll, colorCommands[2, 3].Command);
        Assert.Equal(Command.OutputNumber, colorCommands[2, 4].Command);
        Assert.Equal(Command.Push, colorCommands[2, 5].Command);

        // validate colors
        Assert.Equal(PietColors.LightRed, colorCommands[0, 0].Color);
        Assert.Equal(PietColors.LightYellow, colorCommands[0, 1].Color);
        Assert.Equal(PietColors.LightGreen, colorCommands[0, 2].Color);
        Assert.Equal(PietColors.LightCyan, colorCommands[0, 3].Color);
        Assert.Equal(PietColors.LightBlue, colorCommands[0, 4].Color);
        Assert.Equal(PietColors.LightMagenta, colorCommands[0, 5].Color);

        Assert.Equal(PietColors.Red, colorCommands[1, 0].Color);
        Assert.Equal(PietColors.Yellow, colorCommands[1, 1].Color);
        Assert.Equal(PietColors.Green, colorCommands[1, 2].Color);
        Assert.Equal(PietColors.Cyan, colorCommands[1, 3].Color);
        Assert.Equal(PietColors.Blue, colorCommands[1, 4].Color);
        Assert.Equal(PietColors.Magenta, colorCommands[1, 5].Color);

        Assert.Equal(PietColors.DarkRed, colorCommands[2, 0].Color);
        Assert.Equal(PietColors.DarkYellow, colorCommands[2, 1].Color);
        Assert.Equal(PietColors.DarkGreen, colorCommands[2, 2].Color);
        Assert.Equal(PietColors.DarkCyan, colorCommands[2, 3].Color);
        Assert.Equal(PietColors.DarkBlue, colorCommands[2, 4].Color);
        Assert.Equal(PietColors.DarkMagenta, colorCommands[2, 5].Color);
    }
}