using System.Management;

namespace Nvupd.Core.Helpers;

public static class SystemHelper
{
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
}