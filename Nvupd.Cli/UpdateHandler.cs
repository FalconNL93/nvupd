using Nvupd.Core.Helpers;
using Nvupd.Core.Models;
using ShellProgressBar;

namespace Nvupd.Cli;

public class UpdateHandler
{
    private readonly UpdateData _updateData;
    private readonly GpuInformation _gpuInformation;
    private string TempFile { get; set; }
    private string FileName { get; set; }

    private readonly ProgressBarOptions _progressOptions = new()
    {
        CollapseWhenFinished = true
    };

    public UpdateHandler(UpdateData updateData, GpuInformation gpuInformation)
    {
        _updateData = updateData;
        _gpuInformation = gpuInformation;
    }

    public async Task<string> DownloadUpdate()
    {
        Console.WriteLine($"An update is available for {_gpuInformation.Name}");
        Console.WriteLine("Starting download...");

        return await DownloadDriver();
    }

    private async Task<string> DownloadDriver()
    {
        FileName = Path.GetFileName(_updateData.DownloadUri.LocalPath);
        TempFile = $"{Path.GetTempPath()}{TempFile}.exe";

        await StartDownload();

        return TempFile;
    }

    private async Task StartDownload()
    {
        var progress = new Progress<float>();
        var progressBar = new ProgressBar(10000, "Downloading", _progressOptions);

        progress.ProgressChanged += (sender, p) =>
        {
            var progressFloat = progressBar.AsProgress<float>();
            progressFloat.Report(p);
        };

        progressBar.Message = $"Downloading {_updateData.DownloadUri.LocalPath}...";
        await Downloader.DownloadFile(TempFile, _updateData.DownloadUri.ToString(), progress);
    }
}