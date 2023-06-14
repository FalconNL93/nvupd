using Nvupd.Core.Extensions;

namespace Nvupd.Core.Helpers;

public class Downloader
{
    public static async Task DownloadFile(
        string path,
        string downloadUrl,
        Progress<float> progress,
        CancellationToken cancellationToken = default
    )
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(5);

        await using var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        await client.DownloadAsync(downloadUrl, file, progress, cancellationToken);
    }
}