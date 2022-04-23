using System;
using System.Collections.Generic;
using Piet.Interpreter.Events;
using Xunit;

namespace Piet.Interpreter.UnitTests;

public class OperationTests
{
    [Fact]
    public void Test()
    {
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);
        stack.Push(5);

        var stackAsArray = stack.ToArray();
        Array.Reverse(stackAsArray);

        int numberOfRolls = 2;
        int depthOfRollOperation = 3;

        int index = stackAsArray.Length - depthOfRollOperation;
        for (int i = 0; i < numberOfRolls; i++)
        {
            int temp = stackAsArray[^1];
            Array.Copy(stackAsArray, index, stackAsArray, index + 1, stackAsArray.Length - index -1);
            stackAsArray.SetValue(temp, index);
        }
        stack.Clear();
        foreach (var number in stackAsArray)
        {   
            stack.Push(number);
        }


        Assert.NotNull(stackAsArray);
    }

    [Fact]
    public void Test2()
    {
        var outputEventService = new OutputEventService();

        outputEventService.OutputCharacter += c_OutputCharacter;
        outputEventService.OutputInteger += c_OutputInteger;


        outputEventService.DispatchOutputCharacterEvent('c');
        outputEventService.DispatchOutputIntegerEvent(42);


    }

    static void c_OutputCharacter(object? sender, OutputCharacterOperationEventArgs e)
    {
        Console.WriteLine(e.Value);
    }

    static void c_OutputInteger(object? sender, OutputIntegerOperationEventArgs e)
    {
        Console.WriteLine(e.Value);
    }

}