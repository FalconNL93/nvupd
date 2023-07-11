using Microsoft.Extensions.Options;
using Nvupd.Cli.Models;
using Nvupd.Core.Helpers;
using Nvupd.Core.Services;
using Serilog;

namespace Nvupd.Cli;

public class App
{
    private readonly AppConfigOptions _appConfigOptions;
    private readonly AppOptions _cliOptions;


    public App(IOptions<AppOptions> cliOptions, IOptions<AppConfigOptions> appConfigOptions)
    {
        _appConfigOptions = appConfigOptions.Value;
        _cliOptions = cliOptions.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Log.Information("{AppName} {Version}", Program.AppAssembly.Name, Program.AppVersion);
        Log.Verbose("Temp directory: {TempDirectory}", SystemHelper.TempDirectory);

        var gpu = GpuHelper.GetGpuInformation();
        Log.Information("Device: {GpuName}", gpu.Name);
        Log.Information("Driver installed: {GpuNiceDriverVersion}", gpu.NiceDriverVersion);

        try
        {
            Log.Information("Retrieving update data from NVIDIA...");
            var nvidiaResponse = await NvidiaUpdateService.GetUpdateData();
            Log.Information("Latest driver: {NvidiaResponseVersion}", nvidiaResponse.Version);

            if (!nvidiaResponse.UpdateAvailable && !_cliOptions.ForceUpdate)
            {
                Log.Information("You are running the latest driver");
                return;
            }

            Log.Information("An update is available");
            Log.Information("Driver package: {DownloadUrl}", nvidiaResponse.DownloadUri.ToString());

            var updateHandler = new UpdateHandler(nvidiaResponse, gpu);
            await updateHandler.UpdateAvailable(cancellationToken);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error");
        }
    }
}