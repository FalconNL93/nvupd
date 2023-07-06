using System.CommandLine;

namespace Nvupd.Cli;

public class CliOptions
{
    public bool RunAsService { get; set; }
}

public static class ParameterParser
{
    private static readonly CliOptions CliOptions = new();
    private static readonly RootCommand RootCommand = new("Nvidia Driver Updater");
    private static readonly Option<bool> ServiceOption = new("--service", "Run the updater as service");

    public static async Task<CliOptions> Parse(string[] args)
    {
        RootCommand.AddOption(ServiceOption);
        RootCommand.SetHandler(service => { CliOptions.RunAsService = service; }, ServiceOption);

        await RootCommand.InvokeAsync(args);
        return CliOptions;
    }
}