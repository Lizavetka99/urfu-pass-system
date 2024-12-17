using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UrfuPassSystem.Infrastructure.ImageStorage;

namespace UrfuPassSystem.Tests.Infrastructure;

public class ImageStorageTests
{
    private const string resultPath = "../../../Assets/Result/";
    private const string tempPath = "../../../Assets/Result/Temp/";
    private const string imagesPath = "../../../Assets/Result/Images/";

    private ImageStorage _imageStorage;

    [SetUp]
    public async Task Setup()
    {
        var options = new ImageStorageOptions { TempPath = tempPath, ImagesPath = imagesPath };
        var monitor = Mock.Of<IOptionsMonitor<ImageStorageOptions>>(_ => _.CurrentValue == options);
        var logger = Mock.Of<ILogger<ImageStorage>>();
        _imageStorage = new ImageStorage(monitor, logger);
        await _imageStorage.StartAsync(CancellationToken.None);
        if (Directory.Exists(resultPath))
            Directory.Delete(resultPath, true);
        Directory.CreateDirectory(tempPath);
        Directory.CreateDirectory(imagesPath);
    }

    [TearDown]
    public void TearDown()
    {
        _imageStorage.Dispose();
        if (Directory.Exists(resultPath))
            Directory.Delete(resultPath, true);
    }

    [Test]
    public void TestCreateImageFile()
    {
        var filepath = _imageStorage.CreateImageFile(".test");
        Assert.Multiple(() =>
        {
            Assert.That(Path.GetExtension(filepath), Is.EqualTo(".test"));
            Assert.That(File.Exists(filepath), Is.False);
            Assert.That(Directory.Exists(Path.GetDirectoryName(filepath)), Is.True);
        });
    }

    [Test]
    public void TestCreateTempFolder()
    {
        var folder = _imageStorage.CreateTempFolder(TimeSpan.FromHours(2));
        Assert.Multiple(() =>
        {
            Assert.That(folder.Disposed, Is.False);
            Assert.That(Directory.Exists(folder.Path), Is.True);
            Assert.That(Directory.EnumerateDirectories(folder.Path).Count(), Is.Zero);
            Assert.That(Directory.EnumerateFiles(folder.Path).Count(), Is.Zero);
        });
        folder.Dispose();
        Assert.Multiple(() =>
        {
            Assert.That(folder.Disposed, Is.True);
            Assert.That(Directory.Exists(folder.Path), Is.False);
        });
    }
}
