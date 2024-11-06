using UrfuPassSystem.Domain.Entities;

namespace UrfuPassSystem.App.ImageHandler;

public interface IImageHandler
{
    Task<Image> SaveImage(string rawFilePath);
}
