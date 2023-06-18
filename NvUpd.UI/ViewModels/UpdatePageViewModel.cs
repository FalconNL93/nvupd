using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nvupd.Core.Helpers;
using NvUpd.UI.Models;

namespace NvUpd.UI.ViewModels;

public partial class UpdatePageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private NvidiaUpdate _nvidiaUpdate;

    [ObservableProperty]
    private Progress<float> _progress = new();

    [ObservableProperty]
    private string _status;

    public UpdatePageViewModel()
    {
    }

    [RelayCommand]
    private async Task DownloadUpdate()
    {
        await Downloader.DownloadFile(@"C:\Users\pvand\Downloads\nvidia.exe", _nvidiaUpdate.DownloadUri.ToString(), _progress);
    }
}