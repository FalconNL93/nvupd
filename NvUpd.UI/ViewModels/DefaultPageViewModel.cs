using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nvupd.Core.Models;
using NvUpd.UI.Models;
using NvUpd.UI.Services;

namespace NvUpd.UI.ViewModels;

public partial class DefaultPageViewModel : ObservableRecipient
{
    private readonly GpuService _gpuService;

    [ObservableProperty]
    private GpuInformation _gpuInformation = new();

    [ObservableProperty]
    private NvidiaUpdate _nvidiaUpdate = new();

    public DefaultPageViewModel(GpuService gpuService)
    {
        _gpuService = gpuService;
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

    public async Task CheckForUpdates()
    {
        NvidiaUpdate = await _gpuService.GetLatestUpdates();
    }
}