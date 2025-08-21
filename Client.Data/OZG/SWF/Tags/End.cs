using System.Text;

namespace Client.Data.OZG.SWF.Tags;

public class End : Tag<End>
{

    public End(TagHeader header) : base(header)
    {
    }

    public static End Parse(TagHeader tagHeader)
    {
        return new End(tagHeader);
    }

    public static new End Parse(TagHeader tagHeader, byte[] buffer)
    {
        return End.Parse(tagHeader);
    }

}