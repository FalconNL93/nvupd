using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nvupd.Cli.Models;
using Serilog;

namespace Nvupd.Cli;

internal abstract class Program
{
    public static readonly AssemblyName AppAssembly = Assembly.GetExecutingAssembly().GetName();
    public static readonly string AppVersion = $"{AppAssembly.Version.Major}.{AppAssembly.Version.Minor}.{AppAssembly.Version.Build}";
    private static readonly string? AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static readonly string SettingsFile = @$"{AppDirectory}\settings.json";
    public static event EventHandler? CancelEvent;

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(builder => { builder.AddJsonFile(SettingsFile, optional: true); })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppConfigOptions>(hostContext.Configuration.GetSection("App"));
                services.AddSingleton<App>();
            }).UseSerilog();


    private static async Task Main(string[] args)
    {
        var cancellationToken = new CancellationTokenSource();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Console.Title = $"{AppAssembly.Name} {AppVersion}";
        Console.CancelKeyPress += (_, _) => { OnCancelEvent(); };
        CancelEvent += (_, _) =>
        {
            Log.Information("Stopping...");
            cancellationToken.Cancel();

            Log.Information("Stopped");
            Environment.Exit(0);
        };

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        var host = CreateHostBuilder(args).Build();
        var startup = host.Services.GetRequiredService<App>();

        await startup.StartAsync(cancellationToken.Token);
    }

    public static void OnCancelEvent()
    {
        CancelEvent?.Invoke(null, EventArgs.Empty);
    }
}