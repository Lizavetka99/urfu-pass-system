namespace UrfuPassSystem.Domain.Enums;

/// <summary>Коды результатов проверки фотографий.</summary>
public enum ImageCheckResultCode
{
    /// <summary>Ошибка при проверке.</summary>
    UnexpectedError = -1,
    /// <summary>Проверка успешна.</summary>
    Success = 0,
    /// <summary>Плохое качество изображения.</summary>
    BadQuality = 1,
    /// <summary>Нет лица.</summary>
    NoFace = 2,
    /// <summary>Несколько лиц.</summary>
    MoreThanOneFace = 3,
    /// <summary>Плохо обрезано.</summary>
    BadCrop = 4,
    /// <summary>Неподходящий фон.</summary>
    BadBackground = 5,
    /// <summary>Посторонние объекты на фото.</summary>
    ForeignObjects = 6,
    /// <summary>Неподходящий внешний вид.</summary>
    BadAppearance = 7,
    /// <summary>Неправильное положение лица на фото.</summary>
    BadFace = 8,
    /// <summary>Другая причина.</summary>
    UnexpectedReason = 100,
}
