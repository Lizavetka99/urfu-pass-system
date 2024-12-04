using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UrfuPassSystem.Infrastructure.ImageStorage;

public class ImageStorage : BackgroundService, IImageStorage
{
    private const float errorInMinutes = 5;
    private const float tryToDeleteAgainAfterInMinutes = 60;

    private readonly ImageStorageOptions _options;
    private readonly ILogger<ImageStorage> _logger;

    private readonly List<(DateTime deleteAt, TempFolder folder)> _tempFolders;
    private CancellationTokenSource? _cancTokenSource;

    public ImageStorage(IOptionsMonitor<ImageStorageOptions> options, ILogger<ImageStorage> logger)
    {
        _options = options.CurrentValue;
        _logger = logger;
        options.OnChange(o => _logger.LogWarning("Options change not supported."));
        _tempFolders = [];
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var next = default(DateTime?);
            lock (_tempFolders)
            {
                if (_cancTokenSource is not null && _cancTokenSource.IsCancellationRequested)
                    _cancTokenSource = null;
                for (var i = _tempFolders.Count - 1; i >= 0; i--)
                {
                    var (deleteAt, folder) = _tempFolders[i];
                    if ((deleteAt - now).TotalMinutes > errorInMinutes)
                    {
                        if (!next.HasValue || deleteAt < next)
                            next = deleteAt;
                        continue;
                    }
                    if (!folder.Disposed)
                    {
                        try
                        {
                            folder.UnsafeDispose();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Temp folder deletion exception.");
                            _tempFolders[i] = (now.AddMinutes(tryToDeleteAgainAfterInMinutes), folder);
                            continue;
                        }
                    }
                    _tempFolders.RemoveAt(i);
                }
                _cancTokenSource ??= new CancellationTokenSource();
            }
            var delay = next is not null ? next.Value - now : TimeSpan.FromDays(1);
            try
            {
                await Task.Delay(delay, _cancTokenSource.Token);
            }
            catch { }
        }
    }
    
    public ITempFolder CreateTempFolder(TimeSpan lifetime)
    {
        lock (_tempFolders)
        {
            var now = DateTime.UtcNow;
            var path = RandomTempFolderPath();
            Directory.CreateDirectory(path);
            var folder = new TempFolder(path, now + lifetime);
            _tempFolders.Add((folder.ExpiresAt.AddMinutes(errorInMinutes), folder));
            try
            {
                _cancTokenSource?.Cancel();
            }
            catch { }
            return folder;
        }
    }

    private string RandomTempFolderPath()
    {
        for (var i = 0; i < 100; i++)
        {
            var path = Path.Combine(_options.TempPath, RandomName(_options.TempFolderNameSize));
            if (!Path.Exists(path))
                return path;
        }
        throw new InvalidOperationException("Too many unsuccessful attempts to find empty folder name.");
    }

    public string CreateImageFile(string? extension)
    {
        for (var i = 0; i < 100; i++)
        {
            var subfolderPath = Path.Combine(_options.ImagesPath,
                RandomName(_options.ImagesSubFolderNameSize));
            var path = Path.Combine(subfolderPath,
                RandomName(_options.ImageNameSize));
            var filePath = Path.ChangeExtension(path, extension);
            if (!Path.Exists(filePath))
            {
                if (!Directory.Exists(subfolderPath))
                    Directory.CreateDirectory(subfolderPath);
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
