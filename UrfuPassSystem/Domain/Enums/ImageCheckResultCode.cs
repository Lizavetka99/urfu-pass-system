namespace UrfuPassSystem.Domain.Enums;

/// <summary>Коды результатов проверки фотографий.</summary>
public enum ImageCheckResultCode
{
    /// <summary>Ошибка при проверки.</summary>
    UnexpectedError = -1,
    /// <summary>Проверка успешна.</summary>
    Success = 0,
    /// <summary>Слишком низкое качество изображения.</summary>
    QualityTooLow = 1,

    //..
}
