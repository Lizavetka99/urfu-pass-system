using Microsoft.EntityFrameworkCore;
using UrfuPassSystem.Domain.Entities;
using UrfuPassSystem.Domain.Enums;
using UrfuPassSystem.Domain.Services;
using UrfuPassSystem.Domain.Services.StatisticMaker;

namespace UrfuPassSystem.Tests.Domain;

public class StatisticMakerTests
{
    private ApplicationDbContext _dbContext;
    private IStatisticMaker _statisticMaker;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("inMemory").Options);
        _dbContext.Database.EnsureDeleted();
        _statisticMaker = new StatisticMaker();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task TestEmpty()
    {
        var statistics = await _statisticMaker.MakeImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Total, Is.Zero);
            Assert.That(statistics.Success, Is.Zero);
        });
    }

    [Test]
    public async Task TestSingleSuccessCheck()
    {
        var image = AddImage();
        AddCheck(image, ImageCheckResultCode.Success);
        _dbContext.SaveChanges();
        var statistics = await _statisticMaker.MakeImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Total, Is.EqualTo(1));
            Assert.That(statistics.Success, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task TestSingleNotSuccessCheck()
    {
        var image = AddImage();
        AddCheck(image, ImageCheckResultCode.UnexpectedReason);
        _dbContext.SaveChanges();
        var statistics = await _statisticMaker.MakeImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Total, Is.EqualTo(1));
            Assert.That(statistics.Success, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task TestManySuccessChecks()
    {
        var image = AddImage();
        AddCheck(image, ImageCheckResultCode.Success);
        AddCheck(image, ImageCheckResultCode.Success);
        _dbContext.SaveChanges();
        var statistics = await _statisticMaker.MakeImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Total, Is.EqualTo(1));
            Assert.That(statistics.Success, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task TestManySuccessChecksWithSingleNotSuccess()
    {
        var image = AddImage();
        AddCheck(image, ImageCheckResultCode.Success);
        AddCheck(image, ImageCheckResultCode.UnexpectedReason);
        AddCheck(image, ImageCheckResultCode.Success);
        _dbContext.SaveChanges();
        var statistics = await _statisticMaker.MakeImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Total, Is.EqualTo(1));
            Assert.That(statistics.Success, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task TestManySuccessChecksWithSingleDeletedNotSuccess()
    {
        var image = AddImage();
        AddCheck(image, ImageCheckResultCode.Success);
        AddCheck(image, ImageCheckResultCode.UnexpectedReason).MarkDeleted();
        AddCheck(image, ImageCheckResultCode.Success);
        _dbContext.SaveChanges();
        var statistics = await _statisticMaker.MakeImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Total, Is.EqualTo(1));
            Assert.That(statistics.Success, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task TestManyImages()
    {
        AddCheck(AddImage(), ImageCheckResultCode.Success);
        AddCheck(AddImage(), ImageCheckResultCode.UnexpectedReason);
        AddCheck(AddImage(), ImageCheckResultCode.Success);
        AddCheck(AddImage(), ImageCheckResultCode.Success);
        AddCheck(AddImage(), ImageCheckResultCode.UnexpectedReason);
        _dbContext.SaveChanges();
        var statistics = await _statisticMaker.MakeImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Total, Is.EqualTo(5));
            Assert.That(statistics.Success, Is.EqualTo(3));
        });
    }

    [Test]
    public async Task TestBadStatistics()
    {
        AddCheck(AddImage(), ImageCheckResultCode.Success);
        AddCheck(AddImage(), ImageCheckResultCode.MoreThanOneFace);
        AddCheck(AddImage(), ImageCheckResultCode.UnexpectedReason);
        AddCheck(AddImage(), ImageCheckResultCode.BadBackground);
        AddCheck(AddImage(), ImageCheckResultCode.MoreThanOneFace);
        AddCheck(AddImage(), ImageCheckResultCode.NoFace);
        _dbContext.SaveChanges();
        var statistics = await _statisticMaker.MakeBadImageStatistic(_dbContext, null, null);
        Assert.Multiple(() =>
        {
            Assert.That(statistics.Statistics, Has.Count.EqualTo(4));
            Assert.That(statistics.Statistics.First(s => s.ResultCode == ImageCheckResultCode.MoreThanOneFace).Count, Is.EqualTo(2));
            Assert.That(statistics.Statistics.First(s => s.ResultCode == ImageCheckResultCode.UnexpectedReason).Count, Is.EqualTo(1));
            Assert.That(statistics.Statistics.First(s => s.ResultCode == ImageCheckResultCode.BadBackground).Count, Is.EqualTo(1));
            Assert.That(statistics.Statistics.First(s => s.ResultCode == ImageCheckResultCode.NoFace).Count, Is.EqualTo(1));
        });
    }

    private Image AddImage()
        => _dbContext.Images.Add(new Image { StudentCardId = null, SentTime = DateTime.UtcNow, OriginalFileName = string.Empty, FilePath = string.Empty }).Entity;

    private ImageCheck AddCheck(Image image, ImageCheckResultCode resultCode)
        => _dbContext.ImageChecks.Add(new ImageCheck { Image = image, CheckTime = DateTime.UtcNow, IsAuto = false, IsEdited = true, FilePath = string.Empty, ResultCode = resultCode }).Entity;
}
