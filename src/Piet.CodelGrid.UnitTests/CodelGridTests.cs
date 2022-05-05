using System;
using Piet.Color;
using Xunit;

namespace Piet.Grid.UnitTests
{
    public class CodelGridTests
    {
        [Theory]
        [InlineData(10, 10, 0, 0)]
        [InlineData(10, 10, 0, 9)]
        [InlineData(10, 10, 9, 0)]
        [InlineData(10, 10, 9, 9)]
        public void SetCellColor_ValidInput_MustNotThrow(int gridWidth,
            int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            var codel = grid.GetCodel(xPosition, yPosition);
            Assert.NotNull(codel);

            grid.SetCodelColor(xPosition, yPosition, PietColors.Black);
            codel = grid.GetCodel(xPosition, yPosition);
            Assert.True((PietColor) codel.Color == PietColors.Black);
        }

        [Theory]
        [InlineData(10, 10, -1, 0)]
        [InlineData(10, 10, 0, -1)]
        [InlineData(10, 10, -1, -1)]
        public void SetCellColor_InvalidInput_CoordinatesOutOfRangeAtLowerBound_MustThrow(int gridWidth,
            int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                grid.SetCodelColor(xPosition, yPosition, PietColors.Cyan));
        }

        [Theory]
        [InlineData(10, 10, 0, 10)]
        [InlineData(10, 10, 10, 0)]
        [InlineData(10, 10, 10, 10)]
        public void SetCellColor_InvalidInput_CoordinatesOutOfRangeAtUpperBound_MustThrow(int gridWidth,
            int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                grid.SetCodelColor(xPosition, yPosition, PietColors.Cyan));
        }

        [Theory]
        [InlineData(10, 10, -1, 10)]
        [InlineData(10, 10, 10, -1)]
        public void SetCellColor_InvalidInput_CoordinatesOutOfRangeAtBothLimits_MustThrow(int gridWidth,
            int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                grid.SetCodelColor(xPosition, yPosition, PietColors.Cyan));
        }

        [Theory]
        [InlineData(10, 10, 0, 0)]
        [InlineData(10, 10, 0, 9)]
        [InlineData(10, 10, 9, 0)]
        [InlineData(10, 10, 9, 9)]
        public void SetCodel_ValidInput_MustNotThrow(int gridWidth, int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            var codel = grid.GetCodel(xPosition, yPosition);
            Assert.NotNull(codel);

            grid.SetCodel(new Codel(xPosition, yPosition, PietColors.Black));
            codel = grid.GetCodel(xPosition, yPosition);
            Assert.True((PietColor)codel.Color == PietColors.Black);
        }

        [Theory]
        [InlineData(10, 10, -1, 0)]
        [InlineData(10, 10, 0, -1)]
        [InlineData(10, 10, -1, -1)]
        public void SetCodel_InvalidInput_CoordinatesOutOfRangeAtLowerBound_MustThrow(int gridWidth,
            int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                grid.SetCodel(new Codel(xPosition, yPosition, PietColors.Cyan)));
        }

        [Theory]
        [InlineData(10, 10, 0, 10)]
        [InlineData(10, 10, 10, 0)]
        [InlineData(10, 10, 10, 10)]
        public void SetCodel_InvalidInput_CoordinatesOutOfRangeAtUpperBound_MustThrow(int gridWidth,
            int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                grid.SetCodel(new Codel(xPosition, yPosition, PietColors.Cyan)));
        }

        [Theory]
        [InlineData(10, 10, -1, 10)]
        [InlineData(10, 10, 10, -1)]
        public void SetCodel_InvalidInput_CoordinatesOutOfRangeAtBothLimits_MustThrow(int gridWidth,
            int gridHeight, int xPosition, int yPosition)
        {
            var grid = new CodelGrid(gridHeight, gridWidth, PietColors.White);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                grid.SetCodel(new Codel(xPosition, yPosition, PietColors.Cyan)));
        }
    }
}