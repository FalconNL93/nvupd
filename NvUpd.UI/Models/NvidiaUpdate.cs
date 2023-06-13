using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NvUpd.UI.Models;

public partial class NvidiaUpdate : ObservableRecipient
{
    [ObservableProperty]
    private Uri _downloadUri;

    [ObservableProperty]
    private string _latestVersion;

    [ObservableProperty]
    private bool _updateAvailable;
}