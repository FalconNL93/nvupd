using System.Globalization;
using System.Reflection;
using Nvupd.Cli.Models;
using Serilog;

namespace Nvupd.Cli;

internal abstract class Program
{
    public static readonly AssemblyName AppAssembly = Assembly.GetExecutingAssembly().GetName();
    public static readonly string AppVersion = $"{AppAssembly.Version.Major}.{AppAssembly.Version.Minor}.{AppAssembly.Version.Build}";

    private static CliOptions _cliOptions = new();
    public static event EventHandler? CancelEvent;

    private static async Task Main(string[] args)
    {
        _cliOptions = await ParameterParser.Parse(args);

        var logger = new LoggerConfiguration();
        if (_cliOptions.Verbose)
        {
            logger.MinimumLevel.Verbose();
        }

        Log.Logger = logger
            .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var cancellationToken = new CancellationTokenSource();
        Console.Title = $"{AppAssembly.Name} {AppVersion}";
        Console.CancelKeyPress += (_, _) => { OnCancelEvent(); };
        CancelEvent += (_, _) =>
        {
            Log.Information("Stopping...");
            cancellationToken.Cancel();

            Environment.Exit(0);
        };

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        await App.Run(_cliOptions, cancellationToken);
    }

    public static void OnCancelEvent()
    {
        CancelEvent?.Invoke(null, EventArgs.Empty);
    }
}