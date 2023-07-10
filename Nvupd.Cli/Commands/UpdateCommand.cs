using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Nvupd.Cli.Models;

namespace Nvupd.Cli.Commands;

public class UpdateCommand : Command
{
    private readonly CliOptions _options;
    public new ICommandHandler Handler { get; } = new UpdateCommandHandler();

    private static readonly List<Option> OptionsList = new()
    {
        new Option<bool>("--service", "Run the updater as service"),
        new Option<bool>("--force", "Install latest driver even if already installed"),
        new Option<bool>("--verbose", "Show more verbosity"),
    };

    public UpdateCommand(CliOptions options) : base("update", "Update")
    {
        foreach (var option in OptionsList)
        {
            AddOption(option);
        } 
    }
}

public class UpdateCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var bb = context;

        return 0;
    }
}