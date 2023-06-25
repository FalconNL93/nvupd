using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NvUpd.UI.Configurations;
using NvUpd.UI.Models;
using NvUpd.UI.Services;
using NvUpd.UI.ViewModels;
using NvUpd.UI.Views;
using NvUpd.UI.Views.Pages;

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
            services.AddWindows();
            services.AddPages();

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
        startupForm.ViewModel.Page = AppHost.Services.GetRequiredService<DefaultPage>();
        startupForm.Show();

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

public static class ServiceExtensions
{
    public static void AddWindows(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddTransient<MainWindowViewModel>();
    }

    public static void AddPages(this IServiceCollection services)
    {
        services.AddTransient<UpdatePage>();
        services.AddTransient<UpdatePageViewModel>();
        
        services.AddTransient<DefaultPage>();
        services.AddTransient<DefaultPageViewModel>();
    }
}