using System.Diagnostics;

namespace Nvupd.Core.Helpers;

public static class ExtractHelper
{
    private const string SevenZipExecutable = @"{0}\7-Zip\7z.exe";
    private const string ExtractCommand = @"x {0} -o{1} -y";
    private static readonly string ProgramFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");

    public static async Task Extract(string inputFile, string outputPath)
    {
        if (!File.Exists(inputFile))
        {
            throw new Exception("Driver file does not exist");
        }

        var arguments = string.Format(ExtractCommand, inputFile, outputPath);
        var processInfo = new ProcessStartInfo
        {
            FileName = string.Format(SevenZipExecutable, ProgramFiles),
            Arguments = arguments
        };

        var cancellationToken = new CancellationTokenSource();
        var process = Process.Start(processInfo);
        if (process == null)
        {
            throw new Exception("Error extracting driver file");
        }

        await process.WaitForExitAsync(cancellationToken.Token);
    }
}