using System.ComponentModel.DataAnnotations;

namespace UrfuPassSystem.Data;

public class Image
{
    [Key]
    public Guid Id { get; set; }
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    public DateTime SendTime { get; set; }
    public string? OriginalFileName { get; set; }
    public string? RawFilePath { get; set; }
    public string? ProcessedFilePath { get; set; }
    public bool AutoIsSuccess { get; set; }
    public int AutoResult { get; set; }

    public ImageCheck? Check { get; set; }

    public bool IsDeleted { get; set; }
}
