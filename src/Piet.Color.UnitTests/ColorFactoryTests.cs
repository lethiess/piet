using System;
using System.ComponentModel;
using Xunit;

namespace Piet.Color.UnitTests
{
    public class ColorFactoryTests
    {
        [Fact]
        public void CreateAllPietColors_ValidInput_MustNotThrow()
        {
            var pietColorNameList = Enum.GetValues<PietColorNames>();
            foreach (var colorName in pietColorNameList)
            {
                var color = PietColorFactory.Create(colorName);
                Assert.NotNull(color);
            }
        }

        [Theory]
        [InlineData(0x12, 0x34, 0x45)]
        [InlineData(0x67, 0x89, 0xAB)]
        [InlineData(0xCD, 0xEF, 0x00)]
        [InlineData(0x01, 0x00, 0x00)]
        [InlineData(0x08, 0x15, 0x0F)]
        public void CreateColor_InvalidInput_MustThrow(int red, int green,
            int                                            blue)
        {
            Assert.Throws<InvalidPietColorCodeException>(() =>
                PietColorFactory.Create(red, green, blue));
        }
    }
}