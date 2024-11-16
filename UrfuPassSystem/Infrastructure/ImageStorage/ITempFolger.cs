namespace UrfuPassSystem.Infrastructure.ImageStorage;

/// <summary>Представление временной папки.</summary>
public interface ITempFolger : IDisposable
{
    /// <summary>Путь к папке.</summary>
    string Path { get; }
    /// <summary>Была ли папка удалена со всем содержимым.</summary>
    bool Disposed { get; }
}