namespace Nvupd.Core.Models;

public class GpuInformation
{
    public string Name { get; set; }
    public string DriverVersion { get; set; }
    public string NiceDriverVersion { get; set; }
    public bool IsDch { get; set; }
    public int PfId { get; set; }
    public string OsId { get; set; }
}