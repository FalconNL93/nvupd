using Nvupd.Cli.Models;
using Nvupd.Core.Helpers;
using Nvupd.Core.Services;
using Serilog;

namespace Nvupd.Cli;

public static class App
{
    private static AppConfig AppConfig { get; set; } = new();

    public static async Task Run(CliOptions cliOptions, CancellationTokenSource cancellationToken)
    {
        Log.Information("{AppName} {Version}", Program.AppAssembly.Name, Program.AppVersion);
        Log.Verbose("Temp directory: {TempDirectory}", SystemHelper.TempDirectory);

        try
        {
            AppConfig = ConfigHelper.ReadConfig();
            Log.Verbose("Configuration parsed");
        }
        catch (Exception e)
        {
            AppConfig = new AppConfig();
            Log.Warning("Unable to parse configuration, using default configuration");
        }

        var gpu = GpuHelper.GetGpuInformation();
        Log.Information("Device: {GpuName}", gpu.Name);
        Log.Information("Driver installed: {GpuNiceDriverVersion}", gpu.NiceDriverVersion);

        try
        {
            Log.Information("Retrieving update data from NVIDIA...");
            var nvidiaResponse = await NvidiaUpdateService.GetUpdateData();
            Log.Information("Latest driver: {NvidiaResponseVersion}", nvidiaResponse.Version);

            if (!nvidiaResponse.UpdateAvailable && !cliOptions.ForceUpdate)
            {
                Log.Information("You are running the latest driver");
                return;
            }

            Log.Information("An update is available");
            Log.Information("Driver package: {DownloadUrl}", nvidiaResponse.DownloadUri.ToString());

            var updateHandler = new UpdateHandler(nvidiaResponse, gpu);
            await updateHandler.UpdateAvailable(cancellationToken.Token);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error");
        }
    }
}