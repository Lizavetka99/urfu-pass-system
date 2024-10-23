using UrfuPassSystem.Data;

namespace UrfuPassSystem.ImageHandler;

public interface IImageHandler
{
    Task<Image> SaveImage(string rawFilePath);
}
