using Nvupd.Core.Helpers;
using Nvupd.Core.Services;

namespace Nvupd.Cli;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        
        var gpu = GpuHelper.GetGpuInformation();
        Console.WriteLine($"Device: {gpu.Name}");
        Console.WriteLine($"Driver installed: {gpu.NiceDriverVersion}");
        
        try
        {
            
            var nvidiaResponse = await NvidiaUpdateService.GetUpdateData();
            Console.WriteLine($"Driver available: {nvidiaResponse.Version}");
            
            if (!nvidiaResponse.UpdateAvailable)
            {
                Console.WriteLine("You are running the latest driver.");
                return;
            }
            
            var updateHandler = new UpdateHandler(nvidiaResponse, gpu);
            var driverFile = await updateHandler.DownloadUpdate();
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to fetch data from NVIDIA.");
            Console.WriteLine(e);
        }
        
        Console.ReadKey();
    }
}