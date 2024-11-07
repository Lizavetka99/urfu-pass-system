using UrfuPassSystem.Domain.Enums;
using UrfuPassSystem.Infrastructure;

namespace UrfuPassSystem.Domain.Entities;

/// <summary>Сущность сотрудника.</summary>
public class Employee : Entity
{
    /// <summary>Имя пользователя сотрудника (email).</summary>
    public required string Username { get; init; }
    /// <summary>Id организации, к которой относится сотрудник.</summary>
    public required Guid? OrganizationId { get; init; }
    /// <summary>Уровень доступа сотрудника.</summary>
    public required ModeratorAccessLevel AccessLevel { get; init; }

    /// <summary>Организация, к которой относится сотрудник.</summary>
    public Organization? Organization { get; private set; }
}
