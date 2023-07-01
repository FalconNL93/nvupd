using Nvupd.Core.Helpers;
using Nvupd.Core.Models;
using ShellProgressBar;

namespace Nvupd.Cli;

public class UpdateHandler
{
    private readonly GpuInformation _gpuInformation;
    private Progress<float>? _progress;
    private ProgressBar _progressBar;

    private readonly ProgressBarOptions _progressOptions = new()
    {
        CollapseWhenFinished = true
    };

    private readonly UpdateData _updateData;

    public UpdateHandler(UpdateData updateData, GpuInformation gpuInformation)
    {
        _updateData = updateData;
        _gpuInformation = gpuInformation;
    }

    private string DriverFile { get; set; }
    private string DriverFileOutput { get; set; }

    public async Task UpdateAvailable(CancellationToken cancellationToken)
    {
        Console.WriteLine($"An update is available for {_gpuInformation.Name}.");
        var downloadUpdate = ConsoleX.YesNo("Do you want to download the update?");
        if (!downloadUpdate)
        {
            Console.WriteLine("Cancelled");
            return;
        }

        Console.WriteLine("Starting download...");
        await DownloadDriver(cancellationToken);
        await ExtractPackage();

        Console.WriteLine("Press Y to start driver install");
        var userInput = Console.ReadKey().Key;
        if (userInput != ConsoleKey.Y)
        {
            Environment.Exit(0);
        }

        Console.WriteLine("Starting installation...");
    }

    private async Task DownloadDriver(CancellationToken cancellationToken)
    {
        _progressBar = new ProgressBar(10000, "Downloading", _progressOptions);
        _progress = new Progress<float>();
        _progress.ProgressChanged += ProgressOnProgressChanged;
        DriverFile = Path.GetTempPath() + Guid.NewGuid() + ".exe";
        _progressBar.Message = $"Downloading {_updateData.DownloadUri}";
        await Downloader.DownloadFile(DriverFile, _updateData.DownloadUri.ToString(), _progress, cancellationToken);
        _progressBar.Dispose();
        Console.WriteLine($"File saved to {DriverFile}");
    }

    private async Task ExtractPackage()
    {
        DriverFileOutput = Path.GetTempPath() + Guid.NewGuid();
        await ExtractHelper.Extract(DriverFile, DriverFileOutput);

        Console.WriteLine($"Extracted to {DriverFileOutput}");
    }

    private void ProgressOnProgressChanged(object? sender, float e)
    {
        var progressBar = _progressBar.AsProgress<float>();
        progressBar.Report(e);
    }
}