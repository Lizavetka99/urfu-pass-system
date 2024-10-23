namespace UrfuPassSystem.ArchiveHandler;

public interface IArchiveHandler
{
    Task ExtractArchive(string archivePath, string destinationPath);
}
