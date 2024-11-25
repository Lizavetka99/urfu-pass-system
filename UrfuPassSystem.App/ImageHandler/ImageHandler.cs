using UrfuPassSystem.Domain.Entities;
using UrfuPassSystem.Domain.Enums;
using UrfuPassSystem.Domain.Services;
using UrfuPassSystem.Infrastructure.ImageProcessor;
using UrfuPassSystem.Infrastructure.ImageStorage;

namespace UrfuPassSystem.App.ImageHandler;

public class ImageHandler(ApplicationDbContext database, // TODO: remove database from constructor..
    IImageStorage imageStorage, IImageProcessor processor) : IImageHandler
{
    private readonly ApplicationDbContext _database = database;
    private readonly IImageStorage _imageStorage = imageStorage;
    private readonly IImageProcessor _processor = processor;

    private readonly string _processedImagesExtension = "jpg";

    public async Task<Image> SaveImage(string rawFilePath)
    {
        var originalName = Path.GetFileName(rawFilePath);
        var rawPath = _imageStorage.CreateImageFile(Path.GetExtension(rawFilePath));
        File.Copy(rawFilePath, rawPath);
        var processedPath = _imageStorage.CreateImageFile(_processedImagesExtension);

        var processorResult = await _processor.CheckImage(rawPath, processedPath);

        var isSuccess = processorResult == 0;

        var image = new Image()
        {
            StudentCardId = Random.Shared.Next(1000, 10000).ToString(),
            SentTime = DateTime.UtcNow,
            OriginalFileName = originalName,
            FilePath = rawPath
        };
        var check = new ImageCheck()
        {
            Image = image,
            IsAuto = true,
            IsEdited = true,
            FilePath = processedPath,
            ResultCode = isSuccess ? ImageCheckResultCode.Success : ImageCheckResultCode.UnexpectedError
        };
        await _database.Images.AddAsync(image);
        await _database.ImageChecks.AddAsync(check);
        await _database.SaveChangesAsync();
        return image;
    }
}
