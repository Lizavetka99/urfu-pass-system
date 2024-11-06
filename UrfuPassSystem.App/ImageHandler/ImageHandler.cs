using UrfuPassSystem.App.Data;
using UrfuPassSystem.App.ImageProcessor;
using UrfuPassSystem.App.StudentHandler;

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
            StudentId = student?.Id,
            SendTime = DateTime.UtcNow,
            OriginalFileName = originalName,
            RawFilePath = rawPath,
            ProcessedFilePath = isSuccess ? processedPath : null,
            AutoIsSuccess = isSuccess,
            AutoResult = processorResult
        };
        await _database.Images.AddAsync(image);
        await _database.SaveChangesAsync();
        return image;
    }
}
