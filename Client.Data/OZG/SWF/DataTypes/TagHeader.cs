
namespace Client.Data.OZG.SWF;

public class TagHeader
{
    public ushort RawHeader { get; }
    public int TagCode { get; }
    public int PayloadLength { get; }
    public int TotalHeaderSize { get; } // 2 or 6 bytes depending on format

    private TagHeader(ushort rawHeader, int tagCode, int payloadLength, int headerSize)
    {
        RawHeader = rawHeader;
        TagCode = tagCode;
        PayloadLength = payloadLength;
        TotalHeaderSize = headerSize;
    }

    public TagHeader(int tagCode, int payloadLength, int headerSize)
    {
        RawHeader = 0x00;
        TagCode = tagCode;
        PayloadLength = payloadLength;
        TotalHeaderSize = headerSize;
    } 

    public static TagHeader Parse(BinaryReader reader)
    {
        ushort raw = reader.ReadUInt16();
        int tagCode = raw >> 6;
        int shortLen = raw & 0x3F;

        if (shortLen == 0x3F)
        {
            // Extended format (long header)
            int extendedLength = reader.ReadInt32();
            return new TagHeader(raw, tagCode, extendedLength, 6);
        }
        else
        {
            // Short format
            return new TagHeader(raw, tagCode, shortLen, 2);
        }
    }

    public override string ToString()
    {
        return $"TagCode: {TagCode}, Length: {PayloadLength}, HeaderSize: {TotalHeaderSize}";
    }
}
