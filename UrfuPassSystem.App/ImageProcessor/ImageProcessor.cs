using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace UrfuPassSystem.App.ImageProcessor;

public class ImageProcessor : IImageProcessor
{
    private readonly ILogger<ImageProcessor> _logger;
    private ImageProcessorOptions _options;

    public ImageProcessor(IOptionsMonitor<ImageProcessorOptions> options, ILogger<ImageProcessor> logger)
    {
        _logger = logger;
        _options = options.CurrentValue;
        options.OnChange(newOptions => _options = newOptions);
    }

    public async Task<int> CheckImage(string filePath, string destinationPath)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo(_options.Program, _options.Arguments)
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

        var error = process.StandardError.ReadToEnd();
        if (!string.IsNullOrWhiteSpace(error))
        {
            _logger.LogError("Python processor error: {error}", error);
            return -1;
        }
        var output = process.StandardOutput.ReadToEnd();
        return int.Parse(output);
    }
}
