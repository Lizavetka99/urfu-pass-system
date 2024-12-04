namespace UrfuPassSystem.Infrastructure.ImageStorage;

/// <summary>Хранилище файлов.</summary>
public interface IImageStorage
{
    /// <summary>Создает временную пустую папку, при Dispose полностью удаляет ее и все ее содержимое.</summary>
    /// <param name="lifetime">Время, через которое папка может быть удалена автоматически.</param>
    /// <returns>Объект, представляющий временную папку.</returns>
    ITempFolder CreateTempFolder(TimeSpan lifetime);
    /// <summary>Возвращает путь к файлу, в который можно сохранить изображение.</summary>
    /// <param name="extension">Расширение файла.</param>
    /// <returns>Путь к файлу, в который можно сохранить изображение (по этому пути гарантированно пусто, папка к нему гарантированно сущществует).</returns>
    string CreateImageFile(string? extension);
}
