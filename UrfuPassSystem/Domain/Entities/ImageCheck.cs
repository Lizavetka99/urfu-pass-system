using UrfuPassSystem.Domain.Enums;
using UrfuPassSystem.Infrastructure;

namespace UrfuPassSystem.Domain.Entities;

/// <summary>Сущность проверки фотографии.</summary>
public class ImageCheck : Entity
{
    /// <summary>Id фотографии.</summary>
    public required Guid ImageId { get; init; }
    /// <summary>Время проверки.</summary>
    public DateTime CheckTime { get; set; }
    /// <summary>Проверка автоматическая или ручная (true - автоматическая, false - ручная).</summary>
    public required bool IsAuto { get; init; }
    /// <summary>Id сотрудника, проводившего проверку, если проверка ручная (IsAuto == false).</summary>
    public Guid? EmployeeId { get; init; }
    /// <summary>Была ли фотография изменена при проверке.</summary>
    public required bool IsEdited { get; init; }
    /// <summary>Путь к файлу фотографии на сервере.</summary>
    public required string FilePath { get; init; }

    /// <summary>Код результата проверки.</summary>
    public required ImageCheckResultCode ResultCode { get; init; }

    /// <summary>Была ли проверка успешна.</summary>
    public bool IsSuccess => ResultCode == ImageCheckResultCode.Success;

    /// <summary>Фотография, которую проверяли.</summary>
    public Image? Image { get; private set; }
    /// <summary>Сотрудник, проводивший проверку.</summary>
    public Employee? Employee { get; private set; }
}
