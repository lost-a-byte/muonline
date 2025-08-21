
using Client.Data.OZG.SWF;
using Client.Data.OZG.SWF.Extensions;
using Client.Data.OZG.SWF.Tags;

namespace Client.Data.UnitTests.Gfx;

[TestFixture]
public class DefineSpriteTests
{

    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void TestDefineSprite_01()
    {

        byte[] buffer = {
            0xff, 0x09, 0x06, 0x00, 0x00, 0x00,
            0x41, 0x00, 0x00, 0x00, 0x00, 0x00
        };
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);
        var tagHeader = reader.ReadTagHeader();
        var payload = reader.ReadBytes(tagHeader.PayloadLength);
        var tag = DefineSprite.Parse(tagHeader, payload);

        Assert.That(tag, Is.Not.Null, "Tag should be deserialized!");
    }

    [Test]
    public async Task TestDefineSprite_02()
    {
        byte[] tagHeaderBuffer = { 0xff, 0x09, 0x6a, 0x18, 0x00, 0x00 };
        byte[] buffer = await File.ReadAllBytesAsync(Path.Combine(Constants.CurrentWorkingDirectory, $"ProgressFrame_{39}_{6250}.bin"));
        using var stream = new MemoryStream([.. tagHeaderBuffer, .. buffer]);
        using var reader = new BinaryReader(stream);
        var tagHeader = reader.ReadTagHeader();
        var payload = reader.ReadBytes(tagHeader.PayloadLength);
        var tag = DefineSprite.Parse(tagHeader, payload);
        Assert.That(tag, Is.Not.Null, "Tag should be deserialized!");
    }

    [Test]
    public void TestPlaceObject_01()
    {
        /**
        PlaceObject2 (dpt: 4) (TAG) = 26
        tagIDTagLength (UI16) = 1668
        placeFlagHasClipActions (UB) = 0
        placeFlagHasClipDepth (UB) = 0
        placeFlagHasName (UB) = 0
        placeFlagHasRatio (UB) = 0
        placeFlagHasColorTransform (UB) = 0
        placeFlagHasMatrix (UB) = 1
        placeFlagHasCharacter (UB) = 0
        placeFlagMove (UB) = 1
        depth (UI16) = 4
        matrix (MATRIX)
        hasScale (UB) = 0
        hasRotate (UB) = 0
        NTranslateBits (UB) = 0
        **/

        byte[] buffer = { 0x84, 0x06, 0x05, 0x04, 0x00, 0x00 };
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);
        var tagHeader = reader.ReadTagHeader();
        var payload = reader.ReadBytes(tagHeader.PayloadLength);
        var tag = PlaceObject2.Parse(tagHeader, payload);
        Assert.That(tag.ID, Is.EqualTo(26), "Must be 26");


    }

    [Test]
    public void TestDefineShape_01()
    {
        byte[] buffer = {
            0xbf, 0x00, 0x25,0x00,0x00,0x00, // Header
            0x1e,0x00,0x60,0x00,0x2e,0xe0,0x00,0x15,
            0xe0,0x01,0x43,0x0f,0x00,0xd9,0x40,0x00,
            0x05,0x00,0x00,0x00,0x00,0x10,0x15,0x8b,
            0xb8,0x57,0x9e,0x8a,0x24,0xe5,0xa8,0x9d,
            0x0b,0xb9,0xca,0xaf,0x00
        };
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);
        var tagHeader = reader.ReadTagHeader();
        var payload = reader.ReadBytes(tagHeader.PayloadLength);
        var tag = PlaceObject2.Parse(tagHeader, payload);
        Assert.That(tag.ID, Is.EqualTo(2), "Must be 2");

    }
}