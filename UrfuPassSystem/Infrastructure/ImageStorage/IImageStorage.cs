namespace UrfuPassSystem.Infrastructure.ImageStorage;

/// <summary>Хранилище файлов.</summary>
public interface IImageStorage
{
    /// <summary>Создает временную пустую папку, при Dispose полностью удаляет ее и все ее содержимое.</summary>
    /// <returns>Объект, представляющий временную папку.</returns>
    ITempFolger CreateTempFolger();
    /// <summary>Возвращает путь к файлу, в который можно сохранить изображение.</summary>
    /// <param name="extension">Расширение файла.</param>
    /// <returns>Путь к файлу, в который можно сохранить изображение.</returns>
    string CreateImageFile(string? extension);
}
