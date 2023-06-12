using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
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
    private NvidiaUpdate _nvidiaUpdate = new();

    [ObservableProperty]
    private string _updateStatus;
    
    [ObservableProperty]
    private bool _isBusy;

    public MainWindowViewModel(GpuService gpuService)
    {
        _gpuService = gpuService;
    }

    public async Task Check()
    {
        IsBusy = true;

        try
        {
            GpuInformation = _gpuService.GetGpuInformation();
            var driverInfo = await _gpuService.GetLatestUpdates();
            NvidiaUpdate.UpdateAvailable = driverInfo.UpdateAvailable;
            NvidiaUpdate.LatestVersion = driverInfo.LatestVersion;
        }
        catch (Exception)
        {
            UpdateStatus = "Unable to determine your GPU";
        }
        finally
        {
            IsBusy = false;
        }
    }
}