namespace UrfuPassSystem.App.ImageProcessor;

public interface IImageProcessor
{
    Task<int> CheckImage(string filePath, string destinationPath);
}
