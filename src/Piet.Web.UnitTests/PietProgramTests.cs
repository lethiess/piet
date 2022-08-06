using System.Collections.Generic;
using Piet.Color;
using Piet.Command;
using Piet.Interpreter;
using Piet.Web.Pages;
using Xunit;

namespace Piet.Web.UnitTests
{
    public class PietProgramTests
    {
        public static IEnumerable<object[]> GetCommandInfoTestData()
        {
            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Add),
                                 OperandA = 1,
                                 OperandB = 2,
                                 Value = 3
                             },
                             "Add (1 + 2 = 3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Add),
                                 OperandA = null,
                                 OperandB = null,
                                 Value = null
                             },
                             "Add (NaN + NaN = NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.None),
                                 OperandA = 1,
                                 OperandB = 2,
                                 Value = 3
                             },
                             "None"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.None),
                                 OperandA = null,
                                 OperandB = null,
                                 Value = null
                             },
                             "None"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Push),
                                 Value = 3
                             },
                             "Push (3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Push),
                                 Value = null
                             },
                             "Push (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Pop),
                                 Value = 3
                             },
                             "Pop (3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Pop),
                                 Value = null
                             },
                             "Pop (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Switch),
                                 Value = 3
                             },
                             "Switch (3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Switch),
                                 Value = null
                             },
                             "Switch (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Pointer),
                                 Value = 3
                             },
                             "Pointer (3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Pointer),
                                 Value = null
                             },
                             "Pointer (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.InputCharacter),
                                 Value = 97
                             },
                             "InputCharacter (a)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.InputCharacter),
                                 Value = null
                             },
                             "InputCharacter (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.InputNumber),
                                 Value = 3
                             },
                             "InputNumber (3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.InputNumber),
                                 Value = null
                             },
                             "InputNumber (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.OutputCharacter),
                                 Value = 97
                             },
                             "OutputCharacter (a)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.OutputCharacter),
                                 Value = null
                             },
                             "OutputCharacter (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.OutputNumber),
                                 Value = 3
                             },
                             "OutputNumber (3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.OutputNumber),
                                 Value = null
                             },
                             "OutputNumber (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Duplicate),
                                 Value = 3
                             },
                             "Duplicate (3)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Duplicate),
                                 Value = null
                             },
                             "Duplicate (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Not),
                                 Value = 1
                             },
                             "Not (1)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Not),
                                 Value = null
                             },
                             "Not (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Greater),
                                 Value = 1
                             },
                             "Greater (1)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Greater),
                                 Value = null
                             },
                             "Greater (NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Roll),
                                 OperandA = 1,
                                 OperandB = 2
                             },
                             "Roll (depth: 1, rolls: 2)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Roll),
                                 Value = null
                             },
                             "Roll (depth: NaN, rolls: NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Subtract),
                                 OperandA = 1,
                                 OperandB = 2,
                                 Value = -1
                             },
                             "Subtract (1 - 2 = -1)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Subtract),
                                 OperandA = null,
                                 OperandB = null,
                                 Value = null
                             },
                             "Subtract (NaN - NaN = NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Multiply),
                                 OperandA = 1,
                                 OperandB = 2,
                                 Value = -2
                             },
                             "Multiply (1 * 2 = -2)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Multiply),
                                 OperandA = null,
                                 OperandB = null,
                                 Value = null
                             },
                             "Multiply (NaN * NaN = NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Divide),
                                 OperandA = 10,
                                 OperandB = 2,
                                 Value = 5
                             },
                             "Divide (10 / 2 = 5)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Divide),
                                 OperandA = null,
                                 OperandB = null,
                                 Value = null
                             },
                             "Divide (NaN / NaN = NaN)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Modulo),
                                 OperandA = 10,
                                 OperandB = 2,
                                 Value = 0
                             },
                             "Modulo (10 % 2 = 0)"
                         };

            yield return new object[]
                         {
                             new CommandInfo
                             {
                                 ColorCommand = new ColorCommand(PietColors.Black, Command.Command.Modulo),
                                 OperandA = null,
                                 OperandB = null,
                                 Value = null
                             },
                             "Modulo (NaN % NaN = NaN)"
                         };

        }

        [Theory]
        [MemberData(nameof(GetCommandInfoTestData))]
        public void GetSerializedCommand(CommandInfo commandInfo, string expectedCommandDescription)
        {
            var serializedCommand = PietProgram.GetSerializedCommand(commandInfo);
            Assert.Equal(expectedCommandDescription, serializedCommand);
        }

        [Theory]
        [InlineData(PietInterpreter.CodelChooser.Right, "→")]
        [InlineData(PietInterpreter.CodelChooser.Left, "←")]
        public void MapCodelChooserState(PietInterpreter.CodelChooser codelChooser,
            string expectedMapping)
        {
            var mapping = PietProgram.Map(codelChooser);
            Assert.Equal(expectedMapping, mapping);
        }

        [Theory]
        [InlineData(PietInterpreter.Direction.Right, "→")]
        [InlineData(PietInterpreter.Direction.Left, "←")]
        [InlineData(PietInterpreter.Direction.Up, "↑")]
        [InlineData(PietInterpreter.Direction.Down, "↓")]
        public void MapDirectionPointerState(PietInterpreter.Direction directionPointer,
            string expectedMapping)
        {
            var mapping = PietProgram.Map(directionPointer);
            Assert.Equal(expectedMapping, mapping);
        }
    }
}