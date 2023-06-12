namespace Nvupd.Core.Models;

public class GpuInformation
{
    /// <summary>
    /// Name of the device
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Show full driver version
    /// </summary>
    public string DriverVersion { get; set; }

    /// <summary>
    /// Show short driver version
    /// </summary>
    public string NiceDriverVersion { get; set; }

    /// <summary>
    /// If installed driver is DCH
    /// </summary>
    public bool IsDch { get; set; }

    /// <summary>
    /// Device ID
    /// </summary>
    public int PfId { get; set; }

    /// <summary>
    /// OS ID
    /// </summary>
    public string OsId { get; set; }
}