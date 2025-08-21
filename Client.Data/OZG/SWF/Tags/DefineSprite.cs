using Client.Data.OZG.SWF.Extensions;

namespace Client.Data.OZG.SWF.Tags;

public class DefineSprite(TagHeader header) : Tag<DefineSprite>(header)
{
    public ushort SpriteId;
    public ushort FrameCount;
    public List<Tag> Frames { get; set; } = new();

    public static new DefineSprite Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        ushort spriteId = reader.ReadUInt16();
        ushort frameCount = reader.ReadUInt16();

        List<Tag> frames = new();
        List<int> missingTags = new();

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            try
            {
                Tag tag = reader.ReadTag();
                if (!tag.IsNull)
                {
                    frames.Add(tag);
                }
                else
                {
                    missingTags.Add(tag.ID);
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        return new DefineSprite(tagHeader)
        {
            SpriteId = spriteId,
            FrameCount = frameCount,
            Frames = [.. frames],
        };

    }
}