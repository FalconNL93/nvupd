using System.Threading.Tasks;
using System.Windows.Controls;
using Nvupd.Core.Helpers;
using NvUpd.UI.ViewModels;

namespace NvUpd.UI.Views;

public partial class UpdatePage : Page
{
    public UpdatePage(UpdatePageViewModel viewModel)
    {
        ViewModel = viewModel;
        ViewModel.Progress.ProgressChanged += ProgressOnProgressChanged;
        
        InitializeComponent();
    }

    private void ProgressOnProgressChanged(object? sender, float e)
    {
        DownloadProgress.Value = e;
    }

    public UpdatePageViewModel ViewModel { get; }
}