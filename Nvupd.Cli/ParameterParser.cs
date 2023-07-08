using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Nvupd.Cli.Models;
using Nvupd.Core.Helpers;

namespace Nvupd.Cli;

public static class ParameterParser
{
    private static readonly CliOptions CliOptions = new();
    private static readonly RootCommand RootCommand = new("Nvidia Driver Updater");
    private static readonly Parser CliBuilder;
    private static readonly Option<bool> ServiceOption = new("--service", "Run the updater as service");
    private static readonly Option<bool> ForceUpdate = new("--force", "Install latest driver even if already installed");
    private static readonly Option<bool> Verbose = new("--verbose", "Show more verbosity");

    private static readonly Command Clean = new("clean", "Clean temporary files and directories used by the app");

    static ParameterParser()
    {
        CliBuilder = new CommandLineBuilder(RootCommand).UseDefaults().Build();
    }

    public static async Task<CliOptions> Parse(string[] args)
    {
        RootCommand.AddOption(ServiceOption);
        ServiceOption.AddAlias("-s");

        RootCommand.AddOption(ForceUpdate);
        ForceUpdate.AddAlias("-f");

        RootCommand.AddOption(Verbose);
        Verbose.AddAlias("-v");

        RootCommand.AddCommand(Clean);

        Clean.SetHandler(HandleClean);

        await CliBuilder.InvokeAsync(args);
        return CliOptions;
    }

    private static void HandleClean()
    {
        SystemHelper.CleanTemp();
        Environment.Exit(0);
    }
}