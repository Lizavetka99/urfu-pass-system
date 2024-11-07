using UrfuPassSystem.Infrastructure;

namespace UrfuPassSystem.Domain.Entities;

/// <summary>Сущность студента.</summary>
public class Student : Entity
{
    /// <summary>Номер студенческого билета студента.</summary>
    public required string StudentCardId { get; init; }
    /// <summary>Имя пользователя студента.</summary>
    public required string Username { get; init; }
    /// <summary>Фамилия студента.</summary>
    public required string? LastName { get; init; }
    /// <summary>Имя студента.</summary>
    public required string? FirstName { get; init; }
    /// <summary>Отчество студента.</summary>
    public required string? Patronymic { get; init; }
    /// <summary>Почта студента.</summary>
    public required string? Email { get; init; }
    /// <summary>Id института студента.</summary>
    public required Guid InstituteId { get; init; }
    /// <summary>Группа студента.</summary>
    public required string Group { get; init; }

    /// <summary>Институт студента.</summary>
    public Institute? Institute { get; private set; }
}
