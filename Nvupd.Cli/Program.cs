using Nvupd.Core.Helpers;
using Nvupd.Core.Services;

namespace Nvupd.Cli;

internal class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            var gpu = GpuHelper.GetGpuInformation();
            Console.WriteLine($"Device: {gpu.Name}");
            Console.WriteLine($"Installed Drivers: {gpu.NiceDriverVersion}");
            Console.WriteLine($"OS ID: {gpu.OsId}");
            Console.WriteLine($"GPU ID: {gpu.PfId}");
            Console.WriteLine($"DCH Driver: {gpu.IsDch}");

            Console.WriteLine("Fetching driver information from NVIDIA...");
            try
            {
                var nvidiaResponse = await NvidiaUpdateService.FetchDownloadInfo(gpu.PfId, int.Parse(gpu.OsId), gpu.IsDch);
                Console.WriteLine($"Version: {nvidiaResponse.Version}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to fetch data from NVIDIA.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to fetch latest nvidia updates");
            Console.WriteLine(e);
        }

        Console.ReadKey();
    }
}