

using Client.Data.OZG.SWF;
using Client.Data.OZG.SWF.Extensions;

namespace Client.Data.OZG.SWF.Tags;

public class CSMTextSettings(TagHeader header) : Tag<CSMTextSettings>(header)
{
    public ushort TextID;
    public int UseFlashType;
    public int GridFit;
    public int Reserved;
    public float Thickness;
    public float Sharpness;
    public byte Reserved2;

    public static new CSMTextSettings Parse(TagHeader tagHeader, byte[] buffer)
    {

        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var textID = reader.ReadUInt16();

        var configurationByte = reader.ReadByte();
        var bitReader = new BitReader([configurationByte]);
        var useFlashType = (int)bitReader.ReadUB(2);
        var gridFit = (int)bitReader.ReadUB(3);
        var reserved = (int)bitReader.ReadUB(3);

        var thickness = reader.ReadFloat();
        var sharpness = reader.ReadFloat();
        var reserved2 = reader.ReadByte();
        return new CSMTextSettings(
            header: tagHeader
        )
        {
            TextID = textID,
            UseFlashType = useFlashType,
            GridFit = gridFit,
            Reserved = reserved,
            Thickness = thickness,
            Sharpness = sharpness,
            Reserved2 = reserved2,
        };
    }

}