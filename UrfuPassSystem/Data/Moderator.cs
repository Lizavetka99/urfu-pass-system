using System.ComponentModel.DataAnnotations;

namespace UrfuPassSystem.Data;

public class Moderator
{
    [Key]
    public Guid Id { get; set; }
    public string? UserName { get; set; }
}
