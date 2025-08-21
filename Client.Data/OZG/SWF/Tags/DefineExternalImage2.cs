using System.Text;

namespace Client.Data.OZG.SWF.Tags;

public class DefineExternalImage2 : Tag<DefineExternalImage2>
{
    public ushort ImageID;
    public ushort IdType;
    public ushort BitmapFormat;
    public ushort TargetWidth;
    public ushort TargetHeight;
    public string ExportName;
    public string FileName;
    public byte[] ExtraData;

    public DefineExternalImage2(
        TagHeader header,
        ushort imageID,
        ushort idType,
        ushort bitmapFormat,
        ushort targetWidth,
        ushort targetHeight,
        string exportName,
        string fileName,
        byte[] extraData
    ) : base(header)
    {
        ImageID = imageID;
        IdType = idType;
        BitmapFormat = bitmapFormat;
        TargetWidth = targetWidth;
        TargetHeight = targetHeight;
        ExportName = exportName;
        FileName = fileName;
        ExtraData = extraData;
    }

    public static new DefineExternalImage2 Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var imageID = reader.ReadUInt16();
        var idType = reader.ReadUInt16();
        var bitmapFormat = reader.ReadUInt16();
        var targetWidth = reader.ReadUInt16();
        var targetHeight = reader.ReadUInt16();

        var exportNameLength = reader.ReadSByte();
        var exportNameBuffer = reader.ReadBytes(exportNameLength);

        var fileNameLength = reader.ReadSByte();
        var fileNameBuffer = reader.ReadBytes(fileNameLength);

        var extraDataLength = reader.ReadSByte();
        var extraDataBuffer = reader.ReadBytes(extraDataLength);

        return new DefineExternalImage2(
            header: tagHeader,
            imageID: imageID,
            idType: idType,
            bitmapFormat: bitmapFormat,
            targetWidth: targetWidth,
            targetHeight: targetHeight,
            exportName: Encoding.ASCII.GetString(exportNameBuffer),
            fileName: Encoding.ASCII.GetString(fileNameBuffer),
            extraData: extraDataBuffer
        );
    }

}