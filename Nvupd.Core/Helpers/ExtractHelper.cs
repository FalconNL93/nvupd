using SevenZipExtractor;

namespace Nvupd.Core.Helpers;

public static class ExtractHelper
{
    public static void ExtractDriver(string filePath, string outputPath)
    {
        using var archiveFile = new ArchiveFile(filePath);
        Console.WriteLine(archiveFile.Entries);
    }
}