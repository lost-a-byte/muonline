using Client.Data.ModulusCryptor;
using Client.Data.Texture;
using Client.Data.OZG.SWF;
using Client.Data.OZG.SWF.Tags;
using Client.Data.OZG.SWF.DataTypes;
using Client.Data.OZG;

namespace Client.Data.UnitTests;

[TestFixture]
public class FileDecrypt_Test
{
    private CFXReader cfxReader;
    private OZGReader ozgReader;

    [SetUp]
    public void SetUp()
    {
        cfxReader = new();
        ozgReader = new();
    }

    // [Test]
    public async Task ShouldDeserializeSingleFile()
    {
        var path = Path.Combine(
            Constants.DataPath,
            "Interface",
            "GFx",
            "ProgressFrame.ozg"
        );
        var buffer = await File.ReadAllBytesAsync(path);
        var newBuffer = ModulusCryptor.ModulusCryptor.Decrypt(buffer);

        if (newBuffer is null)
        {
            return;
        }

        await File.WriteAllBytesAsync(Path.Combine(Constants.CurrentWorkingDirectory, "ProgressFrame.gfx"), newBuffer);
    }

    // [Test]
    public async Task ShouldDeserializeCfxFile()
    {
        var path = Path.Combine(
            Constants.DataPath,
            "Interface",
            "GFx",
            "Icons.ozg"
        );

        var exists = File.Exists(path);

        Assert.That(exists, Is.True, "File should exists");

        var cfxData = await cfxReader.Load(path);
        var lastData = cfxData.Tags[422];
        if (lastData is not null && lastData is End endTag)
        {
            Assert.That(endTag.ID, Is.EqualTo(0), "End Tag should have ID = 0");
        }
        Assert.That(cfxData, Is.Not.Null, "Data should be deserialized!");
    }

    [Test]
    public async Task ShouldDeserializeProgressFrameGfxFile()
    {
        var path = Path.Combine(
            Constants.DataPath,
            "Interface",
            "GFx",
            "ProgressFrame.ozg"
        );

        var exists = File.Exists(path);

        Assert.That(exists, Is.True, "File should exists");

        var cfxData = await cfxReader.Load(path);
        var lastData = cfxData.Tags[cfxData.Tags.Length - 1];
        if (lastData is not null && lastData is End endTag)
        {
            Assert.That(endTag.ID, Is.EqualTo(0), "End Tag should have ID = 0");
        }
        if (cfxData.UnresolvedTags.Length > 0)
        {
            foreach (var tag in cfxData.UnresolvedTags)
            {
                await File.WriteAllBytesAsync(Path.Combine(Constants.CurrentWorkingDirectory, $"ProgressFrame_{tag.ID}_{tag.Length}.bin"), tag.Payload);
            }
        }
        Assert.That(cfxData, Is.Not.Null, "Data should be deserialized!");
    }

    // [Test]
    public async Task ShouldDeserializeAnyCfxFiles()
    {
        var filePaths = Directory.GetFiles(
            Constants.DataPath,
            "*.ozg",
            SearchOption.AllDirectories
        );

        foreach (var filePath in filePaths)
        {
            Console.WriteLine(filePath);
            var cfxData = await cfxReader.Load(filePath);
        }

        Assert.That(filePaths, Is.Not.Null, "Should found some ozg files");
    }
}