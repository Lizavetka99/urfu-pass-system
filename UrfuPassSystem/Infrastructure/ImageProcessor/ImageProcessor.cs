using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace UrfuPassSystem.Infrastructure.ImageProcessor;

public class ImageProcessor : IImageProcessor
{
    private readonly ILogger<ImageProcessor> _logger;
    private ImageProcessorOptions _options;

    public ImageProcessor(IOptionsMonitor<ImageProcessorOptions> options, ILogger<ImageProcessor> logger)
    {
        _logger = logger;
        _options = options.CurrentValue;
        options.OnChange(newOptions =>
        {
            _options = newOptions;
            _logger.LogInformation("Options changed.");
        });
    }

    public async Task<ImageProcessorResultCode> CheckImage(string filePath, string destinationPath)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo(_options.Program, _options.Arguments)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            }
        };
        try
        {
            process.Start();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Process start exception.");
            return ImageProcessorResultCode.UnexpectedError;
        }
        try
        {
            process.StandardInput.WriteLine(filePath);
            process.StandardInput.WriteLine(destinationPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Process writeLine exception.");
            return ImageProcessorResultCode.UnexpectedError;
        }
        await process.WaitForExitAsync();

        var error = process.StandardError.ReadToEnd();
        if (!string.IsNullOrWhiteSpace(error))
        {
            _logger.LogError("Python processor error: '{error}'", error);
            return ImageProcessorResultCode.UnexpectedError;
        }

        var output = process.StandardOutput.ReadToEnd();
        if (!int.TryParse(output, out var code)
            || !Enum.IsDefined((ImageProcessorResultCode)code))
        {
            _logger.LogError("Python processor unexpected output: '{output}'", output);
            return ImageProcessorResultCode.UnexpectedError;
        }
        return (ImageProcessorResultCode)code;
    }
}
