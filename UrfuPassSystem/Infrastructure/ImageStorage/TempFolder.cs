
namespace UrfuPassSystem.Infrastructure.ImageStorage;

public class TempFolder(string path, DateTime expiresAt) : ITempFolder
{
    public string Path { get; } = path;
    public DateTime ExpiresAt { get; } = expiresAt;
    public bool Disposed { get; private set; }

    public string CreateEmplySubFolder()
    {
        for (var i = 0; i < 100; i++)
        {
            var path = System.IO.Path.Combine(Path, System.IO.Path.GetRandomFileName());
            if (!System.IO.Path.Exists(path))
            {
                Directory.CreateDirectory(path);
                return path;
            }
        }
        throw new InvalidOperationException("Too many unsuccessful attempts to find empty folder name.");
    }

    public void Dispose()
    {
        try
        {
            Directory.Delete(Path, true);
        }
        catch
        {
            return;
        }
        Disposed = true;
    }

    internal void UnsafeDispose()
    {
        Directory.Delete(Path, true);
        Disposed = true;
    }
}
