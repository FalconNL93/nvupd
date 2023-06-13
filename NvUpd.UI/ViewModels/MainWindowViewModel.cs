using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nvupd.Core.Models;
using NvUpd.UI.Models;
using NvUpd.UI.Services;

namespace NvUpd.UI.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{
    private readonly GpuService _gpuService;

    [ObservableProperty]
    private GpuInformation _gpuInformation = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private NvidiaUpdate _nvidiaUpdate = new();

    [ObservableProperty]
    private string _updateStatus;

    public MainWindowViewModel(GpuService gpuService)
    {
        _gpuService = gpuService;
    }

    private void SetStatus(string status, bool isBusy = false)
    {
        IsBusy = isBusy;
        UpdateStatus = status;
    }

    private void RemoveStatus()
    {
        IsBusy = false;
        UpdateStatus = string.Empty;
    }

    public async Task GetCurrentGpuInformation()
    {
        try
        {
            GpuInformation = GpuService.GetGpuInformation();

            await CheckForUpdates();
        }
        catch (Exception)
        {
            SetStatus("Unable to determine your GPU");
        }
        finally
        {
            RemoveStatus();
        }
    }

    private async Task CheckForUpdates()
    {
        SetStatus("Checking for updates...", true);
        NvidiaUpdate = await _gpuService.GetLatestUpdates();
    }

    [RelayCommand]
    private void OpenUrl(Uri downloadUri)
    {
        Process.Start(new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = downloadUri.ToString()
        });
    }
}