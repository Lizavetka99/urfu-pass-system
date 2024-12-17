using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UrfuPassSystem.Domain.Enums;
using UrfuPassSystem.Domain.Services;
using UrfuPassSystem.Domain.Services.ImageHandler;
using UrfuPassSystem.Infrastructure.ImageProcessor;
using UrfuPassSystem.Infrastructure.ImageStorage;

namespace UrfuPassSystem.Tests.Domain;

public class ImageHandlerTests
{
    private const string photosPath = "../../../Assets/Photos/";
    private const string resultPath = "../../../Assets/Result/";

    private Mock<IImageStorage> _storageMock;
    private Mock<IImageProcessor> _processorMock;
    private ApplicationDbContext _dbContext;
    private IImageHandler _imageHandler;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("inMemory").Options);
        _dbContext.Database.EnsureDeleted();
        _storageMock = new Mock<IImageStorage>();
        _processorMock = new Mock<IImageProcessor>();
        var logger = Mock.Of<ILogger<ImageHandler>>();
        _imageHandler = new ImageHandler(_storageMock.Object, _processorMock.Object, logger);
        if (Directory.Exists(resultPath))
            Directory.Delete(resultPath, true);
        Directory.CreateDirectory(resultPath);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
        if (Directory.Exists(resultPath))
            Directory.Delete(resultPath, true);
    }

    [Test]
    public async Task TestSave()
    {
        _storageMock.SetupSequence(_ => _.CreateImageFile(It.IsAny<string>()))
            .Returns(resultPath + "1.jpeg")
            .Returns(resultPath + "2.jpg");
        _processorMock.SetupSequence(_ => _.CheckImage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(ImageProcessorResultCode.Success));
        var image = await _imageHandler.SaveAndCheckImage(_dbContext, photosPath + "Valid.jpeg", true);
        Assert.Multiple(() =>
        {
            Assert.That(image.OriginalFileName, Is.EqualTo("Valid.jpeg"));
            Assert.That(image.Checks!.Single().IsAuto, Is.True);
            Assert.That(image.Checks!.Single().IsEdited, Is.True);
            Assert.That(image.Checks!.Single().ResultCode, Is.EqualTo(ImageCheckResultCode.Success));
        });
    }

    [Test]
    public async Task TestBadImage()
    {
        _storageMock.SetupSequence(_ => _.CreateImageFile(It.IsAny<string>()))
            .Returns(resultPath + "1.jpeg")
            .Returns(resultPath + "2.jpg");
        _processorMock.SetupSequence(_ => _.CheckImage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(ImageProcessorResultCode.QualityTooLow));
        var image = await _imageHandler.SaveAndCheckImage(_dbContext, photosPath + "Valid.jpeg", true);
        Assert.Multiple(() =>
        {
            Assert.That(image.OriginalFileName, Is.EqualTo("Valid.jpeg"));
            Assert.That(image.Checks!.Single().IsAuto, Is.True);
            Assert.That(image.Checks!.Single().IsEdited, Is.True);
            Assert.That(image.Checks!.Single().ResultCode, Is.EqualTo(ImageCheckResultCode.BadQuality));
        });
    }
}
