using System.Text;
using Client.Data.OZG.SWF.DataTypes;
using Client.Data.OZG.SWF.Extensions;

namespace Client.Data.OZG.SWF.Tags;

public class DefineShape(TagHeader header) : Tag<DefineShape>(header)
{
    public ushort ShapeId;
    public Rect? ShapeBounds = null;
    public ShapeWithStyle? Shapes;


    public static new DefineShape Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        ushort shapeId = reader.ReadUInt16();
        Rect shapeBounds = reader.ReadRect();
        

        return new DefineShape(
            header: tagHeader
        )
        {
        };
    }

}