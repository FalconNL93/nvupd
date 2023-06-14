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
    }

    private async Task DownloadDriver()
    {
        var tempFile = Path.GetTempPath() + Guid.NewGuid() + ".exe";
        _progressBar.Message = $"Downloading {_updateData.DownloadUri}";
        await Downloader.DownloadFile(tempFile, _updateData.DownloadUri.ToString(), _progress);
        _progressBar.Dispose();
        Console.WriteLine($"File saved to {tempFile}");
    }

    private void ProgressOnProgressChanged(object? sender, float e)
    {
        var progressBar = _progressBar.AsProgress<float>();
        progressBar.Report(e);
    }
}