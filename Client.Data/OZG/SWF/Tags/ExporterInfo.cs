using System.Text;

namespace Client.Data.OZG.SWF.Tags;

public class ExporterInfo : Tag<ExporterInfo>
{
    public ushort Version;
    public uint Flags;
    public ushort BitmapFormat;
    public string Prefix;
    public string SwfName;
    public ushort NumCodeOffsets;
    public uint CodeOffset;

    public ExporterInfo(
        TagHeader header,
        ushort version,
        uint flags,
        ushort bitmapFormat,
        string prefix,
        string swfName,
        ushort numCodeOffsets,
        uint codeOffset
    ) : base(header)
    {
        Version = version;
        Flags = flags;
        BitmapFormat = bitmapFormat;
        Prefix = prefix;
        SwfName = swfName;
        NumCodeOffsets = numCodeOffsets;
        CodeOffset = codeOffset;
    }

    public static new ExporterInfo Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var version = reader.ReadUInt16();
        var flags = reader.ReadUInt32();
        var bitmapFormat = reader.ReadUInt16();

        var prefixLength = reader.ReadSByte();
        byte[] prefixPayload = reader.ReadBytes(prefixLength);

        var nameLength = reader.ReadSByte();
        byte[] swfNamePayload = reader.ReadBytes(nameLength);

        var numCodeOffsets = reader.ReadUInt16();
        var codeOffset = reader.ReadUInt32();

        return new ExporterInfo(
            header: tagHeader,
            version: version,
            flags: flags,
            bitmapFormat: bitmapFormat,
            prefix: Encoding.ASCII.GetString(prefixPayload),
            swfName: Encoding.ASCII.GetString(swfNamePayload),
            numCodeOffsets: numCodeOffsets,
            codeOffset: codeOffset
        );
    }

}