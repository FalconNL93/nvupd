using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Nvupd.Cli.Commands;

namespace Nvupd.Cli.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceCollection AddCliCommands(this IServiceCollection services)
    {
        var rootCommand = typeof(RootCommand);
        var commandType = typeof(CliCommand);

        var commands = rootCommand
            .Assembly
            .GetExportedTypes()
            .Where(x => x.Namespace == rootCommand.Namespace && commandType.IsAssignableFrom(x));

        foreach (var command in commands)
        {
            services.AddSingleton(commandType, command);
        }

        return services;
    }
}