using Client.Data.OZG.SWF.DataTypes;
using Client.Data.OZG.SWF.Tags;

namespace Client.Data.OZG;

public class CFX
{
    public required string Type { get; set; }
    public byte Version { get; set; }
    public int Size { get; set; }

    public required Rect DisplayRect { get; set; }
    public float FrameRate { get; set; }
    public ushort FrameCount { get; set; }

    public required Tag[] Tags { get; set; }

    public Tag[] UnresolvedTags { get; set; } = [];

}