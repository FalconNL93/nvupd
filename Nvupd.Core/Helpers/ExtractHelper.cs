using System.Diagnostics;

namespace Nvupd.Core.Helpers;

public static class ExtractHelper
{
    private const string SevenZipExecutable = @"{0}\7-Zip\7z.exe";
    private const string ExtractCommand = @"x {0} -o{1} -y";
    private static readonly string ProgramFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
    private static readonly string UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);


    public static async Task Extract(string inputFile, string outputPath)
    {
        var command = string.Format(ExtractCommand,
            @$"{UserProfile}\Downloads\536.23-desktop-win10-win11-64bit-international-dch-whql.exe",
            @$"{UserProfile}\Downloads\Driver");

        var processInfo = new ProcessStartInfo
        {
            FileName = string.Format(SevenZipExecutable, ProgramFiles),
            Arguments = command
        };

        var cancellationToken = new CancellationToken();
        var process = Process.Start(processInfo);
        if (process == null)
        {
            throw new Exception("Error extracting driver file");
        }

        await process.WaitForExitAsync(cancellationToken);
    }
}