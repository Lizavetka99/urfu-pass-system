using System.ComponentModel.DataAnnotations;

namespace UrfuPassSystem.Domain.Entities;

public class ImageCheck
{
    [Key]
    public Guid Id { get; set; }
    public Guid ImageId { get; set; }
    public Image? Image { get; set; }
    //public Guid ModeratorId { get; set; }
    public DateTime CheckTime { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsEdited { get; set; }
    public string? Message { get; set; }
}
