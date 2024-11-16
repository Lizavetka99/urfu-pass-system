﻿namespace UrfuPassSystem.Domain.Enums;

/// <summary>Коды результатов проверки фотографий.</summary>
public enum ImageCheckResultCode
{
    /// <summary>Ошибка при проверки.</summary>
    UnexpectedError = -1,
    /// <summary>Проверка успешна.</summary>
    Success = 0,
    /// <summary>Слишком низкое качество изображения.</summary>
    QualityTooLow = 1,
    /// <summary>Лицо не обнаружено.</summary>
    FaceNotRecognized = 2,
    /// <summary>Обнаружено несколько лиц.</summary>
    MoreThanOneFaceRecognized = 3,
    /// <summary>Изображение размыто.</summary>
    ImageIsBlurry = 4,
    /// <summary>Неподходящий фон.</summary>
    BadBackground = 5,
    /// <summary>Посторонние объекты на фото.</summary>
    ForeignObjects = 6,
    /// <summary>Неподходящий внешний вид.</summary>
    BadAppearance = 7,
    /// <summary>Неправильное положение лица на фото.</summary>
    BadFace = 8,
    /// <summary>Другая причина.</summary>
    UnexpecredReason = 10,
}
