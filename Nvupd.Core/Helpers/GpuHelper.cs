using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Win32;
using Nvupd.Core.Exceptions;
using Nvupd.Core.Models;

namespace Nvupd.Core.Helpers;

public static class GpuHelper
{
    private const string NvidiaRegistryKey = @"SYSTEM\CurrentControlSet\Services\nvlddmkm";
    private const string ObjectQuery = "select Name, DriverVersion from Win32_VideoController";

    public static GpuInformation GetGpuInformation()
    {
        var gpuInformation = new GpuInformation();

        using var searcher = new ManagementObjectSearcher(ObjectQuery);
        foreach (var o in searcher.Get())
        {
            var obj = (ManagementObject)o;

            gpuInformation.Name = (string)obj["Name"];
            gpuInformation.DriverVersion = (string)obj["DriverVersion"];
            gpuInformation.NiceDriverVersion = ToNiceVersion(gpuInformation.DriverVersion);
        }

        var nvidiaRegistry = Registry.LocalMachine.OpenSubKey(NvidiaRegistryKey, false);
        gpuInformation.IsDch = nvidiaRegistry?.GetValue("DCHUVen") != null;

        try
        {
            var is64Bit = RuntimeInformation.OSArchitecture == Architecture.X64;
            var osCode = $"{Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}";
            using var reader = new StreamReader("Data/os-data.json");
            var json = reader.ReadToEnd();
            var osData = JsonSerializer.Deserialize<JsonNode>(json);
            var osDataItems = osData.Deserialize<OsData[]>();

            var currentOs = osDataItems.FirstOrDefault(x => x.Code == osCode && x.Name.Contains("64"));
            gpuInformation.OsId = currentOs.Id;
        }
        catch (Exception e)
        {
            throw new UnknownOsException();
        }

        try
        {
            var gpuNameId = gpuInformation.Name.Replace("NVIDIA", "").TrimStart();
            using var reader = new StreamReader("Data/gpu-data.json");
            var json = reader.ReadToEnd();
            var gpuData = JsonSerializer.Deserialize<JsonNode>(json);

            if (gpuData == null)
            {
                throw new Exception("Could not parse gpu-data.json");
            }

            var isNotebook = SystemHelper.IsLaptop();
            var systemType = isNotebook ? gpuData["notebook"] : gpuData["desktop"];
            gpuInformation.PfId = int.Parse(systemType[gpuNameId].GetValue<string>());
        }
        catch (Exception e)
        {
            throw new UnknownGpuException();
        }

        return gpuInformation;
    }

    private static decimal ToNiceVersion(string driverVersion)
    {
        var niceVersion = driverVersion.Remove(0, 6).Replace(".", "").Insert(3, ".");

        return Convert.ToDecimal(niceVersion, CultureInfo.InvariantCulture);
    }
}