using System.Text.Json;
using Nvupd.Core.Helpers;
using Nvupd.Core.Models;

namespace Nvupd.Core.Services;

public static class NvidiaUpdateService
{
    private const string Url = "https://gfwsl.geforce.com/services_toolkit/services/com/nvidia/services/AjaxDriverService.php";

    private static readonly HttpClient Client = new() { BaseAddress = new Uri($"{Url}") };

    private static async Task<NvidiaResponse> GetNvidiaInfo(int pfId, int osId, bool dch)
    {
        var dchQuery = dch ? 1 : 0;
        var httpResponse = await Client.GetAsync($"?func=DriverManualLookup&pfid={pfId}&osID={osId}&dch={dchQuery}");
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Invalid status code");
        }

        var response = await httpResponse.Content.ReadAsStringAsync();
        var downloadInfo = JsonSerializer.Deserialize<NvidiaResponse>(response);
        if (downloadInfo == null)
        {
            throw new Exception("Could not read download information");
        }

        return downloadInfo;
    }

    private static async Task<DownloadInfo> GetDownloadInfo(int pfId, int osId, bool dch)
    {
        var downloadInfo = (await GetNvidiaInfo(pfId, osId, dch)).IDS.FirstOrDefault();

        if (downloadInfo == null)
        {
            throw new Exception("Could not fetch nvidia driver information from remote");
        }

        return downloadInfo.downloadInfo;
    }

    public static async Task<UpdateData> GetUpdateData()
    {
        var gpuInformation = GpuHelper.GetGpuInformation();
        var downloadInfo = await GetDownloadInfo(gpuInformation.PfId, int.Parse(gpuInformation.OsId), gpuInformation.IsDch);

        return new UpdateData
        {
            UpdateAvailable = Convert.ToDecimal(downloadInfo.Version) < gpuInformation.NiceDriverVersion,
            Version = downloadInfo.Version,
            NiceVersion = gpuInformation.NiceDriverVersion,
            DownloadUri = new Uri(downloadInfo.DownloadURL)
        };
    }
}