using System.Windows.Controls;
using NvUpd.UI.ViewModels;

namespace NvUpd.UI.Views.Pages;

public partial class DefaultPage : Page
{
    public DefaultPage(DefaultPageViewModel viewModel)
    {
        ViewModel = viewModel;
        
        InitializeComponent();
    }
    
    public DefaultPageViewModel ViewModel { get; }
}