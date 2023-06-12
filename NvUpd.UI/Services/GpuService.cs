using System.Threading.Tasks;
using Nvupd.Core.Helpers;
using Nvupd.Core.Models;
using Nvupd.Core.Services;
using NvUpd.UI.Models;

namespace NvUpd.UI.Services;

public class GpuService
{
    public GpuService()
    {
    }

    public GpuInformation GetGpuInformation()
    {
        return GpuHelper.GetGpuInformation();
    }

    public async Task<NvidiaUpdate> GetLatestUpdates()
    {
        var gpuInformation = GpuHelper.GetGpuInformation();
        var downloadInfo = await NvidiaUpdateService.FetchDownloadInfo(gpuInformation.PfId, int.Parse(gpuInformation.OsId), gpuInformation.IsDch);

        return new NvidiaUpdate
        {
            UpdateAvailable = !NvidiaUpdateService.IsLatest(gpuInformation, downloadInfo),
            LatestVersion = downloadInfo.Version
        };
    }
}