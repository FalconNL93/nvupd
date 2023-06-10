using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NvUpd.UI.ViewModels;
using NvUpd.UI.Views;

namespace NvUpd.UI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        AppHost = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) => { services.AddSingleton<MainWindow>(); }).Build();
    }

    private static IHost? AppHost { get; set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();
        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        startupForm.ViewModel = new MainWindowViewModel();
        startupForm.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        base.OnExit(e);
    }
}