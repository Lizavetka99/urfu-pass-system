namespace UrfuPassSystem.Infrastructure.ArchiveHandler;

/// <summary>Сервис для работы с архивами.</summary>
public interface IArchiveHandler
{
    /// <summary>Извлекает содержимое архива.</summary>
    /// <param name="archivePath">Путь к файлу архива.</param>
    /// <param name="destinationPath">Путь к папке, в которую нужно извлечь архив.</param>
    Task ExtractArchive(string archivePath, string destinationPath);
}
