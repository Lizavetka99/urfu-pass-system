using System.ComponentModel.DataAnnotations;

namespace UrfuPassSystem.Domain.Entities;

public class Moderator
{
    [Key]
    public Guid Id { get; set; }
    public string? UserName { get; set; }
}
