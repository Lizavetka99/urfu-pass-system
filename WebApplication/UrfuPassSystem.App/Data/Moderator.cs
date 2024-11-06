using System.ComponentModel.DataAnnotations;

namespace UrfuPassSystem.App.Data;

public class Moderator
{
    [Key]
    public Guid Id { get; set; }
    public string? UserName { get; set; }
}
