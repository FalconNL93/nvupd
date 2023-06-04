using System.Text.Json;
using Nvupd.Core.Models;

namespace Nvupd.Core.Services;

public class NvidiaUpdateService
{
    private const string Url = "https://gfwsl.geforce.com/services_toolkit/services/com/nvidia/services/AjaxDriverService.php";

    private static readonly HttpClient Client = new() { BaseAddress = new Uri($"{Url}") };

    public static async Task<NvidiaResponse> FetchNvidiaInfo(int pfId, int osId, bool dch)
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

    public static async Task<DownloadInfo> FetchDownloadInfo(int pfId, int osId, bool dch)
    {
        var downloadInfo = (await FetchNvidiaInfo(pfId, osId, dch)).IDS.FirstOrDefault();

        if (downloadInfo == null)
        {
            throw new Exception("Could not fetch nvidia driver information from remote");
        }

        return downloadInfo.downloadInfo;
    }
}