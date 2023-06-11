using CommunityToolkit.Mvvm.ComponentModel;
using Nvupd.Core.Models;
using NvUpd.UI.Models;

namespace NvUpd.UI.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{
    [ObservableProperty]
    private GpuInformation _gpuInformation = new();
    
    [ObservableProperty]
    private NvidiaUpdate _nvidiaUpdate = new();

    [ObservableProperty]
    private string _updateStatus;
}