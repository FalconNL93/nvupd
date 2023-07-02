using System.Globalization;
using System.Reflection;
using Serilog;

namespace Nvupd.Cli;

internal abstract class Program
{
    public static readonly AssemblyName AppAssembly = Assembly.GetExecutingAssembly().GetName();
    public static readonly string AppVersion = $"{AppAssembly.Version.Major}.{AppAssembly.Version.Minor}.{AppAssembly.Version.Build}";
    public static readonly string TempDirectory = Path.GetTempPath() + @$"{AppAssembly.Name}\";
    public static event EventHandler? CancelEvent;

    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var cancellationToken = new CancellationTokenSource();
        Console.Title = $"{AppAssembly.Name} {AppVersion}";
        Console.CancelKeyPress += (_, _) => { OnCancelEvent(); };
        CancelEvent += (_, _) =>
        {
            cancellationToken.Cancel();
            Log.Information("Cancelled");

            Environment.Exit(0);
        };

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        await App.Run(cancellationToken);
    }
    
    public static void OnCancelEvent()
    {
        CancelEvent?.Invoke(null, EventArgs.Empty);
    }
}