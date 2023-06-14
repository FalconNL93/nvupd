using Nvupd.Core.Helpers;
using Nvupd.Core.Services;

namespace Nvupd.Cli;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var gpu = GpuHelper.GetGpuInformation();
        Console.WriteLine($"Device: {gpu.Name}");
        Console.WriteLine($"Installed Drivers: {gpu.NiceDriverVersion}");
        Console.WriteLine($"OS ID: {gpu.OsId}");
        Console.WriteLine($"GPU ID: {gpu.PfId}");
        Console.WriteLine($"DCH Driver: {gpu.IsDch}");
        ConsoleX.WriteHeader();

        Console.WriteLine("Checking for updates...");
        try
        {
            var nvidiaResponse = await NvidiaUpdateService.GetUpdateData();
            Console.WriteLine($"Version: {nvidiaResponse.Version}");

            if (nvidiaResponse.UpdateAvailable)
            {
                var updateHandler = new UpdateHandler(nvidiaResponse, gpu);
                await updateHandler.UpdateAvailable();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to fetch data from NVIDIA.");
        }
        
        Console.ReadKey();
    }
}