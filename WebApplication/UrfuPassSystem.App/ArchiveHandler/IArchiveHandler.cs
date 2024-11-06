namespace UrfuPassSystem.App.ArchiveHandler;

public interface IArchiveHandler
{
    Task ExtractArchive(string archivePath, string destinationPath);
}
