using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using Nvupd.Cli.Models;

namespace Nvupd.Cli.Commands;

public class RootCommand : CliRootCommand
{
    private readonly AppOptions _options = new();

    private static readonly List<CliOption> OptionsList = new()
    {
        new CliOption<string>("name"),
        new CliOption<bool>("force"),
        new CliOption<bool>("verbose"),
    };

    public RootCommand(RootAction action, string description = "") : base(description)
    {
        Action = action;
    }
}

public class RootAction : AsynchronousCliAction
{
    public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = new CancellationToken())
    {
        Console.WriteLine("blablabla");

        return 0;
    }
}