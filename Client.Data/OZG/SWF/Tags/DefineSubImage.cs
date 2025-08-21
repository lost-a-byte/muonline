

using System.Text;

namespace Client.Data.OZG.SWF.Tags;

/**
characterID (UI16) = 1
imageId (UI16) = 0
x1 (UI16) = 264
y1 (UI16) = 60
x2 (UI16) = 284
y2 (UI16) = 88
**/

public class DefineSubImage : Tag<DefineSubImage>
{
    public ushort CharacterID;
    public ushort ImageId;
    public ushort X1;
    public ushort Y1;
    public ushort X2;
    public ushort Y2;

    public DefineSubImage(
        TagHeader header,
        ushort characterID,
        ushort imageId,
        ushort x1,
        ushort y1,
        ushort x2,
        ushort y2
    ) : base(header)
    {
        Header = header;
        CharacterID = characterID;
        ImageId = imageId;
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public static new DefineSubImage Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var characterID = reader.ReadUInt16();
        var imageId = reader.ReadUInt16();
        var x1 = reader.ReadUInt16();
        var y1 = reader.ReadUInt16();
        var x2 = reader.ReadUInt16();
        var y2 = reader.ReadUInt16();

        return new DefineSubImage(
            header: tagHeader,
            characterID: characterID,
            imageId: imageId,
            x1: x1,
            y1: y1,
            x2: x2,
            y2: y2
        );
    }

}