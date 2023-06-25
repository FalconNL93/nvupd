using System.Reflection;
using System.Text.Json;
using Serilog;

namespace Nvupd.Cli;

public class AppConfig
{
    public bool AutoDownload { get; set; }
}

public static class ConfigHelper
{
    private static readonly string? AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static readonly string SettingsFile = @$"{AppDirectory}\settings.json";

    public static AppConfig? ReadConfig()
    {
        var configExists = File.Exists(SettingsFile);
        if (!configExists)
        {
            return new AppConfig();
        }

        var configFile = File.ReadAllText(SettingsFile);
        return JsonSerializer.Deserialize<AppConfig>(configFile);
    }
}