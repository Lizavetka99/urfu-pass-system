namespace UrfuPassSystem.Infrastructure.ImageProcessor;

/// <summary>Параметры запуска процессора.</summary>
public class ImageProcessorOptions
{
    /// <summary>Программа обработки.</summary>
    public required string Program { get; init; }
    /// <summary>Аргументы программы обработки.</summary>
    public required string[] Arguments { get; init; }
}
