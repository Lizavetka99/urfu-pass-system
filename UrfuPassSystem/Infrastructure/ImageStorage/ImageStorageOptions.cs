namespace UrfuPassSystem.Infrastructure.ImageStorage;

/// <summary>Конфигурация хранилища файлов.</summary>
public class ImageStorageOptions
{
    /// <summary>Путь к папке с временными файлами.</summary>
    public string TempPath { get; init; } = "temp";
    /// <summary>Путь к папке с изображениями.</summary>
    public string ImagesPath { get; init; } = "images";
    /// <summary>Алфавит для генерации случайных названий файлов и папок.</summary>
    public string Alphabet { get; init; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    /// <summary>Длина имен подпапок для изображений.</summary>
    public int ImagesSubFolderNameSize { get; init; } = 2;
    /// <summary>Длина имен изображений.</summary>
    public int ImageNameSize { get; init; } = 7;
    /// <summary>Длина имен временных папок.</summary>
    public int TempFolderNameSize { get; init; } = 7;
}
