using System.Text;

namespace Client.Data.OZG.SWF.Tags;

public class Tag<T>(TagHeader header) : Tag(header, false)
{

    public static T Parse(TagHeader tagHeader, byte[] buffer)
    {
        throw new Exception("Not Implemented!");
    }

}

public class Tag
{
    internal TagHeader Header { get; set; }

    public bool IsNull = true;
    public int ID => Header.TagCode;
    public int Length => Header.PayloadLength;

    public byte[] Payload;

    public Tag(TagHeader header, byte[] payload)
    {
        Header = header;
        Payload = payload;
    }
    public Tag(TagHeader header, bool isNull)
    {
        Header = header;
        Payload = [];
        IsNull = isNull;
    }
    public Tag(TagHeader header, byte[] payload, bool isNull)
    {
        Header = header;
        Payload = payload;
        IsNull = isNull;
    }
}