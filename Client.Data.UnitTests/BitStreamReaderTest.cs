
using Client.Data.OZG.SWF.Extensions;

namespace Client.Data.UnitTests;

/**
matrix (MATRIX)
hasScale (UB) = 0
hasRotate (UB) = 0
NTranslateBits (UB) = 7
translateX (SB) = 40
translateY (SB) = 40
*/
[TestFixture]
public class BitStreamReader_Test
{
    private byte[] matrixBytes = { 14, 161, 64, 11 };
    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void TestBitStream()
    {
        using var stream = new MemoryStream(matrixBytes);
        using var reader = new BinaryReader(stream);


        var matrix = reader.ReadMatrix();

        Assert.That(reader.BaseStream.Position, Is.EqualTo(3), "The position must have to be 3");
    }

}