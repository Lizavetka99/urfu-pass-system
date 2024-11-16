using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UrfuPassSystem.Infrastructure.ImageStorage;

public class ImageStorage : IImageStorage
{
    private readonly ImageStorageOptions _options;
    private readonly ILogger<ImageStorage> _logger;

    public ImageStorage(IOptionsMonitor<ImageStorageOptions> options, ILogger<ImageStorage> logger)
    {
        _options = options.CurrentValue;
        _logger = logger;
        options.OnChange(o => _logger.LogWarning("Options change not supported."));
    }

    public ITempFolger CreateTempFolger()
    {
        var path = RandomTempFolgerPath();
        return new TempFolger(path);
    }

    private string RandomTempFolgerPath()
    {
        for (var i = 0; i < 100; i++)
        {
            var path = Path.Combine(_options.TempPath, RandomName(_options.TempFolgerNameSize));
            if (!Path.Exists(path))
                return path;
        }
        throw new InvalidOperationException("Too many unsuccessful attempts to find empty folger name.");
    }

    public string CreateImageFile(string? extension)
    {
        for (var i = 0; i < 100; i++)
        {
            var subfolgerPath = Path.Combine(_options.ImagesPath,
                RandomName(_options.ImagesSubFolgerNameSize));
            var path = Path.Combine(subfolgerPath,
                RandomName(_options.ImageNameSize));
            var filePath = Path.ChangeExtension(path, extension);
            if (!Path.Exists(filePath))
            {
                if (!Directory.Exists(subfolgerPath))
                    Directory.CreateDirectory(subfolgerPath);
                return filePath;
            }
        }
        throw new InvalidOperationException("Too many unsuccessful attempts to find empty image name.");
    }

    private string RandomName(int length)
    {
        var alphabet = _options.Alphabet;
        var line = new char[length];
        for (var i = 0; i < length; i++)
            line[i] = alphabet[Random.Shared.Next(alphabet.Length)];
        return new string(line);
    }
}
