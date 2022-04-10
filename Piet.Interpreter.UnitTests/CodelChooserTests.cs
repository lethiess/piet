using System.Collections.Generic;
using System.Collections.Immutable;
using Piet.Color;
using Piet.Grid;
using Xunit;

namespace Piet.Interpreter.UnitTests
{
    public partial class CodelChooserTests
    {
        private readonly ImmutableList<Codel> _currentCodelBlock = new List<Codel>()
        {
         new(2, 2, PietColors.Red),
         new(2, 3, PietColors.Red),
         new(2, 4, PietColors.Red),
         new(3, 2, PietColors.Red),
         new(3, 3, PietColors.Red),
         new(3, 4, PietColors.Red),
         new(4, 2, PietColors.Red),
         new(4, 3, PietColors.Red),
         new(4, 4, PietColors.Red)
        }.ToImmutableList();

        private CodelGrid GetInitialCodelGrid()
        {
            // Initial codel grid for test cases
            // size: 7x7
            // values: (W := white, R := red)
            // 
            // W W W W W W W 
            // W W W W W W W 
            // W W R R R W W 
            // W W R R R W W 
            // W W R R R W W 
            // W W W W W W W 
            // W W W W W W W 

            var codelGrid = new CodelGrid(7, 7, PietColors.White);
            foreach (var codel in _currentCodelBlock)
            {
                codelGrid.SetCodel(codel);
            }
            
            return codelGrid;
        }

        [Fact]
        public void GetNextCodel_MustNotRotateDirectionPointer_MustToggleCodelChooserOnce_MustMatch()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            // 
            // W W W W W W W
            // W W W W W W W
            // W W R R R B W
            // W W R R R W W
            // W W R R R G W
            // W W W W W W W
            // W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingCodel = new Codel(5, 2, PietColors.Black);
            var expectedNextCodel = new Codel(5, 4, PietColors.Green);
            codelGrid.SetCodel(blockingCodel);
            codelGrid.SetCodel(expectedNextCodel);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.NotNull(nextCodel);
            Assert.Equal(expectedNextCodel, nextCodel);
        }

        [Fact]
        public void GetNextCodel_MustRotateDirectionPointerOnce_ToggleCodelChooserOnce_MustMatch()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            //   | 0 1 2 3 4 5 6
            // --+--------------
            // 0 | W W W W W W W
            // 1 | W W W W W W W
            // 2 | W W R R R B W
            // 3 | W W R R R W W
            // 4 | W W R R R B W
            // 5 | W W G W W W W
            // 6 | W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingFirstGuess = new Codel(5, 2, PietColors.Black);
            var blockingSecondGuess = new Codel(5, 4, PietColors.Black);
            var expectedNextCodel = new Codel(2, 5, PietColors.Green);
            codelGrid.SetCodel(blockingFirstGuess);
            codelGrid.SetCodel(blockingSecondGuess);
            codelGrid.SetCodel(expectedNextCodel);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.NotNull(nextCodel);
            Assert.Equal(expectedNextCodel, nextCodel);
        }

        [Fact]
        public void GetNextCodel_MustRotateDirectionPointerOnce_ToggleCodelChooserTwice_MustMatch()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            //   | 0 1 2 3 4 5 6
            // --+--------------
            // 0 | W W W W W W W
            // 1 | W W W W W W W
            // 2 | W W R R R B W
            // 3 | W W R R R W W
            // 4 | W W R R R B W
            // 5 | W W B W G W W
            // 6 | W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingFirstGuess = new Codel(5, 2, PietColors.Black);
            var blockingSecondGuess = new Codel(5, 4, PietColors.Black);
            var blockingThirdGuess = new Codel(2, 5, PietColors.Black);
            var expectedNextCodel = new Codel(4, 5, PietColors.Green);
            codelGrid.SetCodel(blockingFirstGuess);
            codelGrid.SetCodel(blockingSecondGuess);
            codelGrid.SetCodel(blockingThirdGuess);
            codelGrid.SetCodel(expectedNextCodel);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.NotNull(nextCodel);
            Assert.Equal(expectedNextCodel, nextCodel);
        }

        [Fact]
        public void GetNextCodel_MustRotateDirectionPointerTwice_ToggleCodelChooserTwice_MustMatch()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            //   | 0 1 2 3 4 5 6
            // --+--------------
            // 0 | W W W W W W W
            // 1 | W W W W W W W
            // 2 | W W R R R B W
            // 3 | W W R R R W W
            // 4 | W G R R R B W
            // 5 | W W B W B W W
            // 6 | W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingFirstGuess = new Codel(5, 2, PietColors.Black);
            var blockingSecondGuess = new Codel(5, 4, PietColors.Black);
            var blockingThirdGuess = new Codel(2, 5, PietColors.Black);
            var blockingFourthGuess = new Codel(4, 5, PietColors.Black);
            var expectedNextCodel = new Codel(1, 4, PietColors.Green);
            codelGrid.SetCodel(blockingFirstGuess);
            codelGrid.SetCodel(blockingSecondGuess);
            codelGrid.SetCodel(blockingThirdGuess);
            codelGrid.SetCodel(blockingFourthGuess);
            codelGrid.SetCodel(expectedNextCodel);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.NotNull(nextCodel);
            Assert.Equal(expectedNextCodel, nextCodel);
        }

        [Fact]
        public void GetNextCodel_MustRotateDirectionPointerTwice_ToggleCodelChooserThreeTimes_MustMatch()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            //   | 0 1 2 3 4 5 6
            // --+--------------
            // 0 | W W W W W W W
            // 1 | W W W W W W W
            // 2 | W G R R R B W
            // 3 | W W R R R W W
            // 4 | W B R R R B W
            // 5 | W W B W B W W
            // 6 | W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingFirstGuess = new Codel(5, 2, PietColors.Black);
            var blockingSecondGuess = new Codel(5, 4, PietColors.Black);
            var blockingThirdGuess = new Codel(2, 5, PietColors.Black);
            var blockingFourthGuess = new Codel(4, 5, PietColors.Black);
            var blockingFifthGuess = new Codel(1, 4, PietColors.Black);
            var expectedNextCodel = new Codel(1, 2, PietColors.Green);
            codelGrid.SetCodel(blockingFirstGuess);
            codelGrid.SetCodel(blockingSecondGuess);
            codelGrid.SetCodel(blockingThirdGuess);
            codelGrid.SetCodel(blockingFourthGuess);
            codelGrid.SetCodel(blockingFifthGuess);
            codelGrid.SetCodel(expectedNextCodel);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.NotNull(nextCodel);
            Assert.Equal(expectedNextCodel, nextCodel);
        }

        [Fact]
        public void GetNextCodel_MustRotateDirectionPointerThreeTimes_ToggleCodelChooserThreeTimes_MustMatch()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            //   | 0 1 2 3 4 5 6
            // --+--------------
            // 0 | W W W W W W W
            // 1 | W W W W G W W
            // 2 | W B R R R B W
            // 3 | W W R R R W W
            // 4 | W B R R R B W
            // 5 | W W B W B W W
            // 6 | W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingFirstGuess = new Codel(5, 2, PietColors.Black);
            var blockingSecondGuess = new Codel(5, 4, PietColors.Black);
            var blockingThirdGuess = new Codel(2, 5, PietColors.Black);
            var blockingFourthGuess = new Codel(4, 5, PietColors.Black);
            var blockingFifthGuess = new Codel(1, 4, PietColors.Black);
            var blockingSixthGuess = new Codel(1, 2, PietColors.Black);
            var expectedNextCodel = new Codel(4, 1, PietColors.Green);
            codelGrid.SetCodel(blockingFirstGuess);
            codelGrid.SetCodel(blockingSecondGuess);
            codelGrid.SetCodel(blockingThirdGuess);
            codelGrid.SetCodel(blockingFourthGuess);
            codelGrid.SetCodel(blockingFifthGuess);
            codelGrid.SetCodel(blockingSixthGuess);
            codelGrid.SetCodel(expectedNextCodel);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.NotNull(nextCodel);
            Assert.Equal(expectedNextCodel, nextCodel);
        }


        [Fact]
        public void GetNextCodel_MustRotateDirectionPointerThreeTimes_ToggleCodelChooserFourTimes_MustMatch()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            //   | 0 1 2 3 4 5 6
            // --+--------------
            // 0 | W W W W W W W
            // 1 | W W G W B W W
            // 2 | W B R R R B W
            // 3 | W W R R R W W
            // 4 | W B R R R B W
            // 5 | W W B W B W W
            // 6 | W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingFirstGuess = new Codel(5, 2, PietColors.Black);
            var blockingSecondGuess = new Codel(5, 4, PietColors.Black);
            var blockingThirdGuess = new Codel(2, 5, PietColors.Black);
            var blockingFourthGuess = new Codel(4, 5, PietColors.Black);
            var blockingFifthGuess = new Codel(1, 4, PietColors.Black);
            var blockingSixthGuess = new Codel(1, 2, PietColors.Black);
            var blockingSeventhGuess = new Codel(4, 1, PietColors.Black);
            var expectedNextCodel = new Codel(4, 1, PietColors.Green);
            codelGrid.SetCodel(blockingFirstGuess);
            codelGrid.SetCodel(blockingSecondGuess);
            codelGrid.SetCodel(blockingThirdGuess);
            codelGrid.SetCodel(blockingFourthGuess);
            codelGrid.SetCodel(blockingFifthGuess);
            codelGrid.SetCodel(blockingSixthGuess);
            codelGrid.SetCodel(blockingSeventhGuess);
            codelGrid.SetCodel(expectedNextCodel);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.NotNull(nextCodel);
            Assert.Equal(expectedNextCodel, nextCodel);
        }

        [Fact]
        public void GetNextCodel_NoNextCodelAvailable_MustReturn_NextCodelMustBeNull()
        {
            // codel grid under test
            // values: (W := white, R := red (current codel block), G := green (expected next codel)
            //   | 0 1 2 3 4 5 6
            // --+--------------
            // 0 | W W W W W W W
            // 1 | W W B W B W W
            // 2 | W B R R R B W
            // 3 | W W R R R W W
            // 4 | W B R R R B W
            // 5 | W W B W B W W
            // 6 | W W W W W W W

            var codelGrid = GetInitialCodelGrid();
            var blockingFirstGuess = new Codel(5, 2, PietColors.Black);
            var blockingSecondGuess = new Codel(5, 4, PietColors.Black);
            var blockingThirdGuess = new Codel(2, 5, PietColors.Black);
            var blockingFourthGuess = new Codel(4, 5, PietColors.Black);
            var blockingFifthGuess = new Codel(1, 4, PietColors.Black);
            var blockingSixthGuess = new Codel(1, 2, PietColors.Black);
            var blockingSeventhGuess = new Codel(4, 1, PietColors.Black);
            var blockingEigthGuess = new Codel(4, 1, PietColors.Black);
            codelGrid.SetCodel(blockingFirstGuess);
            codelGrid.SetCodel(blockingSecondGuess);
            codelGrid.SetCodel(blockingThirdGuess);
            codelGrid.SetCodel(blockingFourthGuess);
            codelGrid.SetCodel(blockingFifthGuess);
            codelGrid.SetCodel(blockingSixthGuess);
            codelGrid.SetCodel(blockingSeventhGuess);
            codelGrid.SetCodel(blockingEigthGuess);

            var codelChooser = new CodelChooser(codelGrid);

            PietInterpreter.DirectionPointer = PietInterpreter.Direction.Right;
            PietInterpreter.CodelChooserState = PietInterpreter.CodelChooser.Left;

            var nextCodel = codelChooser.GetNextCodel(_currentCodelBlock);

            Assert.Null(nextCodel);
        }

    }
}
