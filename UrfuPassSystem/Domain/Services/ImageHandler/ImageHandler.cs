using Microsoft.Extensions.Logging;
using UrfuPassSystem.Domain.Entities;
using UrfuPassSystem.Domain.Enums;
using UrfuPassSystem.Infrastructure.ImageProcessor;
using UrfuPassSystem.Infrastructure.ImageStorage;

namespace UrfuPassSystem.Domain.Services.ImageHandler;

public class ImageHandler(IImageStorage imageStorage, IImageProcessor processor, ILogger<ImageHandler> logger) : IImageHandler
{
    private readonly IImageStorage _imageStorage = imageStorage;
    private readonly IImageProcessor _processor = processor;
    private readonly ILogger<ImageHandler> _logger = logger;

    private readonly string _processedImagesExtension = "jpg";

    public async Task<Image> SaveAndCheckImage(ApplicationDbContext dbContext, string rawFilePath)
    {
        var originalName = Path.GetFileName(rawFilePath);
        var originalPath = _imageStorage.CreateImageFile(Path.GetExtension(rawFilePath));
        File.Copy(rawFilePath, originalPath);
        var processedPath = _imageStorage.CreateImageFile(_processedImagesExtension);

        var processorResult = await _processor.CheckImage(originalPath, processedPath);

        var resultCode = ProcessorCodeToCheckCode(processorResult);

        var image = new Image()
        {
            StudentCardId = null,
            SentTime = DateTime.UtcNow,
            OriginalFileName = originalName,
            FilePath = originalPath
        };
        var check = new ImageCheck()
        {
            Image = image,
            CheckTime = DateTime.UtcNow,
            IsAuto = true,
            IsEdited = true,
            FilePath = resultCode == ImageCheckResultCode.Success ? processedPath : originalPath,
            ResultCode = resultCode
        };
        dbContext.Images.Add(image);
        dbContext.ImageChecks.Add(check);
        await dbContext.SaveChangesAsync();
        _logger.LogDebug("Image '{path}' added and checked with code {code}.", rawFilePath, resultCode);
        return image;
    }

    private static ImageCheckResultCode ProcessorCodeToCheckCode(ImageProcessorResultCode code)
    {
        if (code < 0)
            return ImageCheckResultCode.UnexpectedError;
        return (ImageCheckResultCode)code;
    }
}
