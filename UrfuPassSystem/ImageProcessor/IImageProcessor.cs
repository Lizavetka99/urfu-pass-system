namespace UrfuPassSystem.ImageProcessor;

public interface IImageProcessor
{
    Task<int> CheckImage(string filePath, string destinationPath);
}
