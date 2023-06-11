using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nvupd.Core.Services;
using NvUpd.UI.Models;
using NvUpd.UI.Services;
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
        AppHost = Host.CreateDefaultBuilder().ConfigureServices((_, services) =>
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<MainWindowViewModel>();

            services.AddSingleton<GpuService>();
            services.AddSingleton<NvidiaUpdate>();
        }).Build();
    }

    private static IHost? AppHost { get; set; }

    protected override async void OnStartup(StartupEventArgs e)
    {
        if (AppHost == null)
        {
            throw new Exception("No AppHost");
        }

        await AppHost.StartAsync();
        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        var gpuService = AppHost.Services.GetRequiredService<GpuService>();

        startupForm.Show();
        startupForm.ViewModel.GpuInformation = gpuService.GetGpuInformation();
        
        startupForm.ViewModel.UpdateStatus = "Searching for updates...";
        var driverInfo = await gpuService.GetLatestUpdates();
        startupForm.ViewModel.NvidiaUpdate.UpdateAvailable = driverInfo.UpdateAvailable;
        startupForm.ViewModel.NvidiaUpdate.LatestVersion = driverInfo.LatestVersion;
        startupForm.ViewModel.UpdateStatus = "Done";

        await Task.Delay(5000);
        startupForm.ViewModel.NvidiaUpdate.UpdateAvailable = true;
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (AppHost == null)
        {
            throw new Exception("No AppHost");
        }

        await AppHost.StopAsync();
        base.OnExit(e);
    }
}