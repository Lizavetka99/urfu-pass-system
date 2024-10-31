using System.Diagnostics;

namespace UrfuPassSystem.ImageProcessor;

public class ImageProcessor(ILogger<ImageProcessor> logger) : IImageProcessor
{
    private readonly ILogger<ImageProcessor> _logger = logger;

    public async Task<int> CheckImage(string filePath, string destinationPath)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo("python.exe", "python-image-handler/RunImageHandler.py")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true
            }
        };
        process.Start();
        process.StandardInput.WriteLine(filePath);
        process.StandardInput.WriteLine(destinationPath);

        await process.WaitForExitAsync();
        
        string error = process.StandardError.ReadToEnd();
        if (!string.IsNullOrWhiteSpace(error))
        {
            _logger.LogError("Python processor error: {error}", error);
            return -1;
        }
        string output = process.StandardOutput.ReadToEnd();
        return int.Parse(output);
    }
}
