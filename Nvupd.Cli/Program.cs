using System.Globalization;
using Nvupd.Core.Helpers;
using Nvupd.Core.Services;
using Serilog;

namespace Nvupd.Cli;

internal class Program
{
    private static AppConfig AppConfig { get; set; } = new();


    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        
        Log.Information("Reading configuration");
        await Task.Delay(3000);

        try
        {
            AppConfig = ConfigHelper.ReadConfig() ?? new AppConfig();
        }
        catch (Exception e)
        {
            Log.Warning("Unable to parse configuration, using default configuration");
        }

        var cancellationToken = new CancellationTokenSource();
        Console.CancelKeyPress += (_, _) => { cancellationToken.Cancel(); };
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        var gpu = GpuHelper.GetGpuInformation();
        Log.Information("Device: {GpuName}", gpu.Name);
        Log.Information("Driver installed: {GpuNiceDriverVersion}", gpu.NiceDriverVersion);

        try
        {
            var nvidiaResponse = await NvidiaUpdateService.GetUpdateData();
            Log.Information("Driver available: {NvidiaResponseVersion}", nvidiaResponse.Version);

            if (!nvidiaResponse.UpdateAvailable)
            {
                Log.Information("You are running the latest driver");
                return;
            }

            if (!AppConfig.AutoDownload)
            {
                return;
            }

            var updateHandler = new UpdateHandler(nvidiaResponse, gpu);
            await updateHandler.UpdateAvailable(cancellationToken.Token);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error");
        }

        Console.ReadKey();
    }
}