using System.Runtime.InteropServices;

namespace Nvupd.Core.Apis;

public static partial class WinApi
{
    [LibraryImport("user32.dll", EntryPoint = "MessageBoxA", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int Messagebox(IntPtr hWnd, string text, string caption, uint type);
}