namespace Nvupd.Core.Models;

public class UpdateData
{
    public bool UpdateAvailable { get; set; }
    public string Version { get; set; }
    public decimal NiceVersion { get; set; }
    public Uri DownloadUri { get; set; }
}