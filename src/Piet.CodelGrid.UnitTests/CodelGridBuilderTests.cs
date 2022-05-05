using System;
using Piet.Color;
using Xunit;

namespace Piet.Grid.UnitTests
{
    public class CodelGridBuilderTests
    {
        [Fact]
        public void Build_ValidGirdBuilderConfiguration_MustMatch()
        {
            var gridBuilder = new CodelGridBuilder();

            int width = 10;
            int height = 10;
            PietColor initialColor = PietColors.Black;

            var codelGrid = gridBuilder.WithHeight(height)
                .WithWidth(width)
                .WithInitialColor(initialColor)
                .Build();

            Assert.NotNull(codelGrid);
            Assert.Equal(height, codelGrid.Height);
            Assert.Equal(width, codelGrid.Width);
        }

        [Fact]
        public void Build_ValidInput_NoInitialColorPassed_MustMatch()
        {
            var gridBuilder = new CodelGridBuilder();

            int width = 10;
            int height = 10;

            var codelGrid = gridBuilder.WithHeight(height)
                .WithWidth(width)
                .Build();

            Assert.NotNull(codelGrid);
            Assert.NotNull(codelGrid);
            Assert.Equal(height, codelGrid.Height);
            Assert.Equal(width, codelGrid.Width);
        }

        [Fact]
        public void Build_InvalidInput_MissingHeight_MustThrow()
        {
            var gridBuilder = new CodelGridBuilder();

            int width = 10;
            gridBuilder = gridBuilder.WithWidth(width);

            Assert.Throws<ArgumentOutOfRangeException>(() => gridBuilder.Build());
        }

        [Fact]
        public void Build_InvalidInput_MissingWidth_MustThrow()
        {
            var gridBuilder = new CodelGridBuilder();

            int height = 10;
            gridBuilder = gridBuilder.WithHeight(height);

            Assert.Throws<ArgumentOutOfRangeException>(() => gridBuilder.Build());
        }
    }
}