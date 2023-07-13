using System.Management;

namespace Nvupd.Core.Helpers;

public static class SystemHelper
{
    public static readonly string TempDirectory = Path.GetTempPath() + @"NvidiaUpdater\";

    public static bool IsLaptop()
    {
        var systemEnclosures = new ManagementClass("Win32_SystemEnclosure");
        foreach (var o in systemEnclosures.GetInstances())
        {
            var obj = (ManagementObject)o;
            foreach (int chassisType in (ushort[])obj["ChassisTypes"])
            {
                return chassisType == 10;
            }
        }

        return false;
    }

    public static void CleanTemp()
    {
        var tempDirectories = new DirectoryInfo(TempDirectory);
        foreach (var directory in tempDirectories.EnumerateDirectories())
        {
            Console.WriteLine($"Deleting directory: {directory}");
        }

        foreach (var file in tempDirectories.EnumerateFiles())
        {
            Console.WriteLine($"Deleting file: {file}");
        }
    }

    public static void Messagebox()
    {
        Apis.WinApi.Messagebox(IntPtr.Zero, "Test", "Test", 1);
    }
}