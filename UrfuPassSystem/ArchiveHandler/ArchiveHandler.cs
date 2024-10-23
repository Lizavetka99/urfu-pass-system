using System.IO.Compression;

namespace UrfuPassSystem.ArchiveHandler;

public class ArchiveHandler : IArchiveHandler
{
    public Task ExtractArchive(string archivePath, string destinationPath)
    {
        if (Path.GetExtension(archivePath) != ".zip")
            throw new NotSupportedException("Only .zip archive supported.");
        ZipFile.ExtractToDirectory(archivePath, destinationPath);
        return Task.CompletedTask;
    }
}
