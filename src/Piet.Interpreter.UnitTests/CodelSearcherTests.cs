using System.Collections.Generic;
using System.Collections.Immutable;
using Piet.Color;
using Piet.Grid;
using Xunit;

namespace Piet.Interpreter.UnitTests
{
    public class CodelSearcherTests
    {
        public static IEnumerable<object[]> GetCodelArrays()
        {
            // Required format for the test data:
            //
            // CodelArray: Codel[,]
            // Height: int
            // Width: int
            // SeedCodel: Codel
            // CodelBlock: List<Codel>

            yield return new object[]
                         {
                             // codel grid (X := seed codel color, O =: other codel color):
                             // O O O
                             // X X X
                             // O X O

                             // expected codel block:
                             // 
                             // X X X  
                             //   X

                             new Codel[,]
                             {
                                 { new (0, 0, PietColors.White), new (1, 0, PietColors.White), new (2, 0, PietColors.White)},
                                 { new (0, 1, PietColors.Red), new (1, 1, PietColors.Red), new (2, 1, PietColors.Red)},
                                 { new (0, 2, PietColors.White), new (1, 2, PietColors.Red), new (2, 2, PietColors.White)},
                             },
                             3, // height
                             3, // width
                             new Codel(1, 1, PietColors.Red),
                             new List<Codel>()
                             {
                                 new(1,1, PietColors.Red),
                                 new(1,0, PietColors.Red),
                                 new(0,1, PietColors.Red),
                                 new(2,1, PietColors.Red),
                             }
                         };

            yield return new object[]
                         {
                             // codel grid (X := seed codel color, O =: other codel color):
                             // X X X
                             // O O O
                             // X O X

                             // expected codel block
                             // X X X
                             //   
                             //     
                             
                             new Codel[,]
                             {
                                 { new (0, 0, PietColors.White), new (1, 0, PietColors.White), new (2, 0, PietColors.White)},
                                 { new (0, 1, PietColors.Red), new (1, 1, PietColors.Red), new (2, 1, PietColors.Red)},
                                 { new (0, 2, PietColors.White), new (1, 2, PietColors.Red), new (2, 2, PietColors.White)},
                             },
                             3, // height
                             3, // width
                             new Codel(1, 0, PietColors.White),
                             new List<Codel>()
                             {
                                 new(0,0, PietColors.White),
                                 new(1,0, PietColors.White),
                                 new(2,0, PietColors.White),
                             }
                         };

            yield return new object[]
                         {
                             // codel grid (X := seed codel color, O =: other codel color):
                             // X X X
                             // X O X
                             // O O X
                             // X X O

                             // expected codel block:
                             // X X X
                             // X   X
                             //     X
                             //  

                             new Codel[,]
                             {
                                 { new (0, 0, PietColors.LightMagenta), new (1, 0, PietColors.LightMagenta), new (2, 0, PietColors.LightMagenta)},
                                 { new (0, 1, PietColors.LightMagenta), new (1, 1, PietColors.Red), new (2, 1, PietColors.LightMagenta)},
                                 { new (0, 2, PietColors.White), new (1, 2, PietColors.Red), new (2, 2, PietColors.LightMagenta)},
                                 { new (0, 3, PietColors.LightMagenta), new (1, 3, PietColors.LightMagenta), new (2, 3, PietColors.Blue)},
                             },
                             4, // height
                             3, // width
                             new Codel(1, 0, PietColors.LightMagenta),
                             new List<Codel>()
                             {
                                 new(0,0, PietColors.LightMagenta),
                                 new(1,0, PietColors.LightMagenta),
                                 new(2,0, PietColors.LightMagenta),
                                 new(0,1, PietColors.LightMagenta),
                                 new(1,2, PietColors.LightMagenta),
                                 new(2,2, PietColors.LightMagenta),
                             }
                         };

            yield return new object[]
                         {
                             // codel grid (X := seed codel color, O =: other codel color):
                             // X O X
                             // O X O
                             // X O X


                             // expected codel block:
                             // 
                             //   X
                             //

                             new Codel[,]
                             {
                                 { new (0, 0, PietColors.Blue), new (1, 0, PietColors.Red), new (2, 0, PietColors.Blue)},
                                 { new (0, 1, PietColors.Red), new (1, 1, PietColors.Blue), new (2, 1, PietColors.Red)},
                                 { new (0, 2, PietColors.Blue), new (1, 2, PietColors.Red), new (2, 2, PietColors.Blue)},
                             },
                             4, // height
                             3, // width
                             new Codel(1, 1, PietColors.Blue),
                             new List<Codel>()
                             {
                                 new(1,1, PietColors.Blue),
                             }
                         };


        }

        [Theory]
        [MemberData(nameof(GetCodelArrays))]
        public void GetCodelBlock_MustMatch(Codel[,] codelArray, int height, int width, Codel seedCodel, List<Codel> expectedCodelBlock)
        {
            var codelGrids = new CodelGrid(height, width, codelArray);

            var codelBlockSearcher = new CodelBlockSearcher(codelGrids);

            var codelBlock = codelBlockSearcher.GetCodelBock(seedCodel).ToImmutableList();

            Assert.NotNull(codelBlock);
            Assert.Equal(expectedCodelBlock.Count, codelBlock.Count);
            Assert.All(codelBlock, codel => expectedCodelBlock.Contains(codel));
        }
    }
}