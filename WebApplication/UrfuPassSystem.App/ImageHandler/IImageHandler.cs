using UrfuPassSystem.App.Data;

namespace UrfuPassSystem.App.ImageHandler;

public interface IImageHandler
{
    Task<Image> SaveImage(string rawFilePath);
}
