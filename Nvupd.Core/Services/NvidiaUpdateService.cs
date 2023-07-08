using System.Diagnostics;
using System.Text.Json;
using Nvupd.Core.Helpers;
using Nvupd.Core.Models;
using Serilog;

namespace Nvupd.Core.Services;

public static class NvidiaUpdateService
{
    private const string Url = "https://gfwsl.geforce.com/services_toolkit/services/com/nvidia/services/AjaxDriverService.php";
    private const string QueryString = "?func=DriverManualLookup&pfid={0}&osID={1}&dch={2}";

    private static readonly HttpClient Client = new() { BaseAddress = new Uri($"{Url}") };

    private static async Task<NvidiaResponse> GetNvidiaInfo(int pfId, int osId, bool dch)
    {
        var dchQuery = dch ? 1 : 0;
        var httpResponse = await Client.GetAsync(string.Format(QueryString, pfId, osId, dchQuery));
        Log.Verbose("Request URI: {RequestUri}", httpResponse.RequestMessage.RequestUri);
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception("Invalid status code");
        }
        
        var response = await httpResponse.Content.ReadAsStringAsync();
        Log.Verbose("Response: {Response}", response);
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
        var versionDecimal = decimal.Parse(downloadInfo.Version);

        return new UpdateData
        {
            UpdateAvailable = gpuInformation.NiceDriverVersion != versionDecimal,
            Version = downloadInfo.Version,
            NiceVersion = gpuInformation.NiceDriverVersion,
            DownloadUri = new Uri(downloadInfo.DownloadURL)
        };
    }
}