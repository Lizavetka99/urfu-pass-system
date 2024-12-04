using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using System.IO.Compression;

namespace UrfuPassSystem.Infrastructure.ArchiveHandler;

public class ArchiveHandler : IArchiveHandler
{
    public Task ExtractArchive(string archivePath, string destinationPath)
    {
        if (Path.GetExtension(archivePath) == ".zip")
        {
            ZipFile.ExtractToDirectory(archivePath, destinationPath);
            return Task.CompletedTask;
        }
        else if (Path.GetExtension(archivePath) == ".rar")
        {
            using var archive = RarArchive.Open(archivePath);
            archive.ExtractToDirectory(destinationPath);
            return Task.CompletedTask;
        }
        else
            throw new NotSupportedException("Only .zip and .rar archive supported.");
    }

    public Task FolderToZip(string folderPath, string archivePath)
    {
        if (Path.GetExtension(archivePath) != ".zip")
            throw new NotSupportedException("Only .zip archive supported.");
        ZipFile.CreateFromDirectory(folderPath, archivePath);
        return Task.CompletedTask;
    }
}
