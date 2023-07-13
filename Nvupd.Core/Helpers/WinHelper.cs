namespace Nvupd.Core.Helpers;

public static class WinHelper
{
    public static MessageBoxResults MessageBox(string text, string caption, IEnumerable<uint> types)
    {
        return (MessageBoxResults)Apis.WinApi.Messagebox(IntPtr.Zero, text, caption, Convert.ToUInt32(types.Sum(x => x)));
    }

    public static MessageBoxResults MessageBox(string text, string caption = "", uint type = MessageBoxTypes.Ok)
    {
        return MessageBox(text, caption, new[] { type });
    }
}

public class MessageBoxTypes
{
    public const uint Ok = 0x0;
    public const uint YesNo = 0x00000004;

    public const uint Error = 0x00000010;
    public const uint Question = 0x00000020;
    public const uint Exclamation = 0x00000030;
    public const uint Information = 0x00000040;
    public const uint SystemModal = 0x00001000;
    public const uint TopMost = 0x00040000;
    public const uint TextRight = 0x00080000;
}

public enum MessageBoxResults
{
    Ok = 1,
    Cancel = 2,
    Abort = 3,
    Yes = 6,
    No = 7
}