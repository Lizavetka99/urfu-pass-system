using UrfuPassSystem.Infrastructure.ArchiveHandler;

namespace UrfuPassSystem.Tests.Infrastructure;

public class ArchiveHandlerTests
{
    private const string archivePath = "../../../Assets/Archives/";
    private const string resultPath = "../../../Assets/Result/";

    private IArchiveHandler _archiveHandler;

    [SetUp]
    public void Setup()
    {
        _archiveHandler = new ArchiveHandler();
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
    public async Task TestZip()
    {
        await _archiveHandler.ExtractArchive(archivePath + "tests.zip", resultPath);
        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(resultPath + "tests/1 (3).png"), Is.True);
            Assert.That(File.Exists(resultPath + "tests/1 (1).jpg"), Is.True);
            Assert.That(File.Exists(resultPath + "tests/1 (1).jpeg"), Is.True);
        });
    }

    [Test]
    public async Task TestRar()
    {
        await _archiveHandler.ExtractArchive(archivePath + "tests.rar", resultPath);
        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(resultPath + "tests/tests/1 (3).png"), Is.True);
            Assert.That(File.Exists(resultPath + "tests/tests/1 (1).jpg"), Is.True);
            Assert.That(File.Exists(resultPath + "tests/tests/1 (1).jpeg"), Is.True);
        });
    }
}
