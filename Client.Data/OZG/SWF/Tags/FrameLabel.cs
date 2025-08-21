using System.Text;
using Client.Data.OZG.SWF.Extensions;

namespace Client.Data.OZG.SWF.Tags;

public class FrameLabel(TagHeader header) : Tag<FrameLabel>(header)
{
    public string Name = "";

    public static new FrameLabel Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        string name = reader.ReadNullTerminatedString();
        return new FrameLabel(
            header: tagHeader
        )
        {
            Name = name,
        };
    }

}