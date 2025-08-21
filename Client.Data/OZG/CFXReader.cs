using System.IO.Compression;
using Client.Data.OZG.SWF;
using Client.Data.OZG.SWF.DataTypes;
using Client.Data.OZG.SWF.Extensions;
using Client.Data.OZG.SWF.Tags;

namespace Client.Data.OZG;

public class CFXReader : BaseReader<CFX>
{
    protected override CFX Read(byte[] buffer)
    {
        buffer = ModulusCryptor.ModulusCryptor.Decrypt(buffer);

        using var ms = new MemoryStream(buffer);
        using var br = new BinaryReader(ms);

        var type = br.ReadString(3);
        if (!(type is "CFX" || type is "GFX"))
        {
            throw new Exception("This file is not a cfx file");
        }

        var version = br.ReadByte();
        var size = br.ReadInt32();

        byte[] rawData;
        switch (type)
        {
            case "CFX":
                {
                    using var uncompressed = new MemoryStream();
                    using var dec = new ZLibStream(ms, CompressionMode.Decompress);
                    dec.CopyTo(uncompressed);

                    rawData = uncompressed.ToArray();
                    break;
                }
            case "GFX":
                rawData = br.ReadBytes(size);
                break;
            default:
                throw new Exception("This file is not a cfx file");
        }


        using var memoryStream = new MemoryStream(rawData);
        using var binaryReader = new BinaryReader(memoryStream);

        
        var displayRect = binaryReader.ReadRect();
        var frameRateBytes = binaryReader.ReadBytes(2);
        var frameRate = Fixed8.Parse_LE(frameRateBytes[0], frameRateBytes[1]);
        var frameCount = binaryReader.ReadUInt16();

        var tags = new List<Tag>();

        var unresolvedTags = new List<Tag>();


        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
        {
            Tag tag = binaryReader.ReadTag();
            
            if (!tag.IsNull)
            {
                tags.Add(tag);
            }
            else
            {
                unresolvedTags.Add(tag);
            }
        }
        return new CFX
        {
            Type = type,
            Version = version,
            Size = size,
            DisplayRect = displayRect,
            FrameRate = frameRate,
            FrameCount = frameCount,
            UnresolvedTags = unresolvedTags.ToArray(),
            Tags = tags.ToArray(),
        };
    }
}