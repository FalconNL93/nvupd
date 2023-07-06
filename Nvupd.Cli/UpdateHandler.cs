using System.Diagnostics;
using Nvupd.Core.Helpers;
using Nvupd.Core.Models;
using Serilog;
using ShellProgressBar;

namespace Nvupd.Cli;

public class UpdateHandler
{
    private readonly GpuInformation _gpuInformation;

    private readonly ProgressBarOptions _progressOptions = new()
    {
        CollapseWhenFinished = true
    };

    private readonly UpdateData _updateData;
    private Progress<float>? _progress;
    private ProgressBar _progressBar;

    public UpdateHandler(UpdateData updateData, GpuInformation gpuInformation)
    {
        _updateData = updateData;
        _gpuInformation = gpuInformation;
    }

    private string DriverFile { get; set; }
    private string DriverFileOutput { get; set; }

    public async Task UpdateAvailable(CancellationToken cancellationToken)
    {
        Log.Information("Starting download...");
        await DownloadDriver(cancellationToken);

        Log.Information("Extracting driver...");
        var outputDirectory = await ExtractPackage();
        Log.Information("Driver package extracted to: {OutputDirectory}", outputDirectory);

        var startInstaller = ConsoleX.YesNo("Do you want to install the driver?");
        if (!startInstaller)
        {
            Program.OnCancelEvent();
            return;
        }

        Log.Information("Starting installation...");
        await InstallDriver(new DriverComponents(new[]
        {
            Components.Driver
        }), cancellationToken);
    }

    private async Task InstallDriver(
        DriverComponents components,
        CancellationToken cancellationTokenSource
    )
    {
        var processInfo = new ProcessStartInfo
        {
            Arguments = "-s -n",
            FileName = @$"{DriverFileOutput}\setup.exe",
            WorkingDirectory = DriverFileOutput
        };

        if (!File.Exists(processInfo.FileName))
        {
            Log.Error("Unable to find {FileName}", processInfo.FileName);
        }

        Log.Information("Running installer...");
        var process = new Process { StartInfo = processInfo };
        process.Start();
        await process.WaitForExitAsync(cancellationTokenSource);

        Log.Information("Installation exited with code {ExitCode}", process.ExitCode);
    }

    private async Task DownloadDriver(CancellationToken cancellationToken)
    {
        _progressBar = new ProgressBar(10000, "Downloading", _progressOptions);
        _progress = new Progress<float>();
        _progress.ProgressChanged += ProgressOnProgressChanged;
        DriverFile = Program.TempDirectory + Guid.NewGuid() + ".exe";

        var driverDirectory = Path.GetDirectoryName(DriverFile);
        if (!Directory.Exists(driverDirectory))
        {
            try
            {
                Directory.CreateDirectory(driverDirectory);
            }
            catch (Exception e)
            {
                Log.Error(e, "Unable to create directory", driverDirectory);
            }
        }

        _progressBar.Message = $"Downloading {_updateData.DownloadUri}";
        await Downloader.DownloadFile(DriverFile, _updateData.DownloadUri.ToString(), _progress, cancellationToken);
        _progressBar.Dispose();
        Log.Information("File saved to {DriverFile}", DriverFile);
    }

    private async Task<string> ExtractPackage()
    {
        DriverFileOutput = Program.TempDirectory + Guid.NewGuid();
        await ExtractHelper.Extract(DriverFile, DriverFileOutput);

        return DriverFileOutput;
    }

    private void ProgressOnProgressChanged(object? sender, float e)
    {
        var progressBar = _progressBar.AsProgress<float>();
        progressBar.Report(e);
    }
}