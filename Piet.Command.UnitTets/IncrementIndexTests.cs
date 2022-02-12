using Xunit;

namespace Piet.Command.UnitTests
{
    public class IncrementIndexTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 4)]
        [InlineData(4, 5)]
        [InlineData(5, 0)]
        public void IncrementHueIndex_ValidInput_MustMatch(int currentIndex, int expected)
        {
            var incrementedIndex = PietCommandControl.IncrementHueIndex(currentIndex);
            Assert.Equal(expected, incrementedIndex);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 0)]
        public void IncrementSatuationIndex_ValidInput_MustMatch(int currentIndex, int expected)
        {
            var incrementedIndex = PietCommandControl.IncrementSatuationIndex(currentIndex);
            Assert.Equal(expected, incrementedIndex);
        }
    }
}