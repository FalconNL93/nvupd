using CommunityToolkit.Mvvm.ComponentModel;

namespace NvUpd.UI.Models;

public partial class NvidiaUpdate : ObservableRecipient
{
    [ObservableProperty]
    private bool _updateAvailable;

    [ObservableProperty]
    private string _latestVersion;
}