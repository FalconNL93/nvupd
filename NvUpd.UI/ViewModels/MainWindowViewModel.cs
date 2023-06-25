using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nvupd.Core.Models;
using NvUpd.UI.Models;
using NvUpd.UI.Services;
using NvUpd.UI.Views;

namespace NvUpd.UI.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{
    public string AppTitle { get; } = "Nvidia Updater";

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private Page _page;
}