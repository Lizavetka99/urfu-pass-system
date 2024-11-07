using UrfuPassSystem.Infrastructure;

namespace UrfuPassSystem.Domain.Entities;

/// <summary>Сущность института.</summary>
public class Institute : Entity
{
    public required string Name { get; init; }
}
