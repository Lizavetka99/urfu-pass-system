namespace UrfuPassSystem.Infrastructure.ImageProcessor;

/// <summary>Процессор обработки фотографии.</summary>
public interface IImageProcessor
{
    /// <summary>Проверяет фотографию.</summary>
    /// <param name="filePath">Путь к исходной фотографии.</param>
    /// <param name="destinationPath">Путь, в который нужно сохранить обработанную фотографию.</param>
    /// <returns>Код результата обработки фотографии.</returns>
    Task<ImageProcessorResultCode> CheckImage(string filePath, string destinationPath);
}
