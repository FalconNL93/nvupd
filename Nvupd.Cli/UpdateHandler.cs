using Nvupd.Core.Helpers;
using Nvupd.Core.Models;
using ShellProgressBar;

namespace Nvupd.Cli;

public class UpdateHandler
{
    private readonly UpdateData _updateData;
    private readonly GpuInformation _gpuInformation;
    private readonly ProgressBar _progressBar;
    private readonly Progress<float> _progress;

    private readonly ProgressBarOptions _progressOptions = new()
    {
        CollapseWhenFinished = true
    };

    private string DriverFile { get; set; }
    private string DriverFileOutput { get; set; }

    public UpdateHandler(UpdateData updateData, GpuInformation gpuInformation)
    {
        _updateData = updateData;
        _gpuInformation = gpuInformation;
        _progressBar = new ProgressBar(10000, "Downloading", _progressOptions);
        _progress = new Progress<float>();
        _progress.ProgressChanged += ProgressOnProgressChanged;
    }

    public async Task UpdateAvailable()
    {
        Console.WriteLine($"An update is available for {_gpuInformation.Name}");
        Console.WriteLine("Starting download...");

        await DownloadDriver();
        await ExtractPackage();
    }

    private async Task DownloadDriver()
    {
        DriverFile = Path.GetTempPath() + Guid.NewGuid() + ".exe";
        _progressBar.Message = $"Downloading {_updateData.DownloadUri}";
        await Downloader.DownloadFile(DriverFile, _updateData.DownloadUri.ToString(), _progress);
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