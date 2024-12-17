using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UrfuPassSystem.Infrastructure.ImageProcessor;

namespace UrfuPassSystem.Tests.Infrastructure;

public class ImageProcessorTests
{
    private const string photosPath = "../../../Assets/Photos/";
    private const string resultPath = "../../../Assets/Result/";
    private const string falseRun = "../../../Assets/falserun.py";
    private const string trueRun = "../../../Assets/truerun.py";

    private IImageProcessor _falseImageProcessor;
    private IImageProcessor _trueImageProcessor;

    [SetUp]
    public void Setup()
    {
        var options = new ImageProcessorOptions { Program = "python3", Arguments = [falseRun] };
        var monitor = Mock.Of<IOptionsMonitor<ImageProcessorOptions>>(_ => _.CurrentValue == options);
        var logger = Mock.Of<ILogger<ImageProcessor>>();
        _falseImageProcessor = new ImageProcessor(monitor, logger);
        options = new ImageProcessorOptions { Program = "python3", Arguments = [trueRun] };
        monitor = Mock.Of<IOptionsMonitor<ImageProcessorOptions>>(_ => _.CurrentValue == options);
         logger = Mock.Of<ILogger<ImageProcessor>>();
        _trueImageProcessor = new ImageProcessor(monitor, logger);
        if (Directory.Exists(resultPath))
            Directory.Delete(resultPath, true);
        Directory.CreateDirectory(resultPath);
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(resultPath))
            Directory.Delete(resultPath, true);
    }

    [Test]
    public async Task TestValid()
    {
        var result = await _trueImageProcessor.CheckImage(photosPath + "Valid.jpeg", resultPath + "Valid.jpeg");
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(ImageProcessorResultCode.Success));
            Assert.That(File.Exists(resultPath + "Valid.jpeg"), Is.True);
        });
    }

    [Test]
    public async Task TestInvalid()
    {
        var result = await _falseImageProcessor.CheckImage(photosPath + "Invalid.jpeg", resultPath + "Invalid.jpeg");
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.EqualTo(ImageProcessorResultCode.Success));
            Assert.That(File.Exists(resultPath + "Invalid.jpeg"), Is.False);
        });
    }
}
