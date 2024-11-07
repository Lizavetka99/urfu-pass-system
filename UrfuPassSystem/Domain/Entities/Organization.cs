using UrfuPassSystem.Infrastructure;

namespace UrfuPassSystem.Domain.Entities;

/// <summary>Сущность организации.</summary>
public class Organization : Entity
{
    /// <summary>Название организации.</summary>
    public required string Name { get; init; }

    /// <summary>Сотрудники организации.</summary>
    public IReadOnlyCollection<Employee>? Employees { get; private set; }
    /// <summary>Институты, к фотографиям студентов из которых имеет доступ организация.</summary>
    public IReadOnlyCollection<Institute>? Institutes { get; private set; }
}
