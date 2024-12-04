namespace UrfuPassSystem.Infrastructure.ImageStorage;

/// <summary>Представление временной папки.</summary>
public interface ITempFolder : IDisposable
{
    /// <summary>Путь к папке.</summary>
    string Path { get; }
    /// <summary>Время, когда папка может быть удалена автоматически.</summary>
    DateTime ExpiresAt { get; }
    /// <summary>Была ли папка удалена со всем содержимым.</summary>
    bool Disposed { get; }

    /// <summary>Создает пустую подпапку.</summary>
    /// <returns>Путь к созданной подпапке.</returns>
    public string CreateEmplySubFolder();
}
