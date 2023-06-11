using NvUpd.UI.ViewModels;

namespace NvUpd.UI.Views;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        DataContext = ViewModel;

        InitializeComponent();
    }

    public MainWindowViewModel ViewModel { get; } = new();
}