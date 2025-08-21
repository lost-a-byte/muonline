using System.Text;

namespace Client.Data.OZG.SWF.Tags;

public class DoAction(TagHeader header) : Tag<DoAction>(header)
{
    public required byte[] ActionBytes;

    public static new DoAction Parse(TagHeader tagHeader, byte[] buffer)
    {
        return new DoAction(
            header: tagHeader
        )
        {
            ActionBytes = buffer
        };
    }

}