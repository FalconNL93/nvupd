using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Nvupd.Cli.Commands;

namespace Nvupd.Cli.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceCollection AddCliCommands(this IServiceCollection services)
    {
        var greetCommandType = typeof(UpdateCommand);
        var commandType = typeof(Command);

        var commands = greetCommandType
            .Assembly
            .GetExportedTypes()
            .Where(x => x.Namespace == greetCommandType.Namespace && commandType.IsAssignableFrom(x));

        foreach (var command in commands)
        {
            services.AddSingleton(commandType, command);
        }

        return services;
    }
}