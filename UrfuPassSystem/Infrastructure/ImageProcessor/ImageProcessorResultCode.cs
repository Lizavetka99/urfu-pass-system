namespace UrfuPassSystem.Infrastructure.ImageProcessor;

/// <summary>Код результата обработки фотографии процессором.</summary>
public enum ImageProcessorResultCode
{
    /// <summary>Необрабатываемая ошибка.</summary>
    UnexpectedError = -3,
    /// <summary>Папка не найдена.</summary>
    FolgerNotFound = -2,
    /// <summary>Файл не найден.</summary>
    FileNotFound = -1,
    /// <summary>Успешно.</summary>
    Success = 0,
    /// <summary>Слишком низкое качество изображения.</summary>
    QualityTooLow = 1,
    /// <summary>Лицо не обнаружено.</summary>
    FaceNotRecognized = 2,
    /// <summary>Обнаружено несколько лиц.</summary>
    MoreThanOneFaceRecognized = 3,
    /// <summary>Изображение размыто.</summary>
    ImageIsBlurry = 4
}
