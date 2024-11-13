using UrfuPassSystem.App.ImageProcessor;
using UrfuPassSystem.App.StudentHandler;
using UrfuPassSystem.Domain.Entities;
using UrfuPassSystem.Domain.Enums;
using UrfuPassSystem.Domain.Services;

namespace UrfuPassSystem.App.ImageHandler;

public class ImageHandler(ApplicationDbContext database,
    IStudentHandler studentHandler, IImageProcessor processor) : IImageHandler
{
    private readonly ApplicationDbContext _database = database;
    private readonly IStudentHandler _studentHandler = studentHandler;
    private readonly IImageProcessor _processor = processor;

    private readonly string _rawImagesPath = "images/raw";
    private readonly string _processedImagesPath = "images/processed";
    private readonly string _processedImagesExtension = "jpg";

    public async Task<Image> SaveImage(string rawFilePath)
    {
        var originalName = Path.GetFileName(rawFilePath);
        // TODO: check if exist
        var rawName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(rawFilePath));
        var rawPath = Path.Combine(_rawImagesPath, rawName);
        var processedName = Path.ChangeExtension(Path.GetRandomFileName(), _processedImagesExtension);
        var processedPath = Path.Combine(_processedImagesPath, processedName);

        var student = await _studentHandler.CreateStudentFromFileName(originalName);

        File.Copy(rawFilePath, rawPath);

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
