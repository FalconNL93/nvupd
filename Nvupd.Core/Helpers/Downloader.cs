namespace Nvupd.Core.Helpers;

public class Downloader
{
    public static async Task DownloadFile(
        string path,
        string downloadUrl,
        Progress<float>? progress,
        CancellationToken cancellationToken = default
    )
    {
        var httpClient = new HttpClient();
        var httpResult = await httpClient.GetAsync(downloadUrl, cancellationToken);
        await using var resultStream = await httpResult.Content.ReadAsStreamAsync(cancellationToken);
        await using var fileStream = File.Create(path);
        await resultStream.CopyToAsync(fileStream, cancellationToken);
    }
}