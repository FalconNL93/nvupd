using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NvUpd.UI.Configurations;
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

            services.AddAutoMapper(typeof(AutoMapperConfiguration));
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

        startupForm.Show();
        await startupForm.ViewModel.GetCurrentGpuInformation();

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