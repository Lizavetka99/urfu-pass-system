namespace UrfuPassSystem.Infrastructure.ImageStorage;

public class TempFolger(string path) : ITempFolger
{
    public string Path { get; } = path;
    public bool Disposed { get; private set; }

    public void Dispose()
    {
        Directory.Delete(Path, true);
        Disposed = true;
    }
}
