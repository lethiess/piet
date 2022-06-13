namespace Piet.Interpreter.Output;

public class OutputCommandLogEventArg : EventArgs
{
    public CommandInfo CommandInfo { get; set; }

    public OutputCommandLogEventArg(CommandInfo commandInfo)
    {
        CommandInfo = commandInfo;
    }
}