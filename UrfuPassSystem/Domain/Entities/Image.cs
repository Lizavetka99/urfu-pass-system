using UrfuPassSystem.Infrastructure;

namespace UrfuPassSystem.Domain.Entities;

/// <summary>Сущность фотографии, отправленной студентом.</summary>
public class Image : Entity
{
    /// <summary>Номер студенческого билета студента, который отправил фотографию.</summary>
    public required string StudentCardId { get; init; }
    /// <summary>Время отправки фотографии студентом.</summary>
    public required DateTime SentTime { get; init; }
    /// <summary>Изначальное название файла фотографии (с расширением).</summary>
    public required string OriginalFileName { get; init; }
    /// <summary>Путь к исходному файлу фотографии на сервере.</summary>
    public required string FilePath { get; init; }
    /// <summary>Файл был удален с сервера.</summary>
    public bool IsDeleted { get; private set; }

    /// <summary>Проверки фотографии.</summary>
    public IReadOnlyCollection<ImageCheck>? Checks { get; private set; }

    /// <summary>Помечает файл, как удаленный.</summary>
    public Image MarkDeleted()
    {
        IsDeleted = true;
        return this;
    }
}
