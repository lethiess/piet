using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Piet.Color;
using Piet.Grid;
using Xunit;

namespace Piet.Interpreter.UnitTests
{
    public class CodelSearcherTests
    {
        public static IEnumerable<object[]> GetCodelArrays()
        {
            yield return new object[]
                         {
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