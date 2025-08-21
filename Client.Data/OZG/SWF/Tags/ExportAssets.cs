using System.Text;
using Client.Data.OZG.SWF.Extensions;

namespace Client.Data.OZG.SWF.Tags;

public class ExportedAsset
{
    public ushort CharacterId { get; set; }
    public required string Name { get; set; }

    
}

/**
tagLength (SI32) = 12
count (UI16) = 1
characterId (UI16) = 2
name (string)
*/
public class ExportAssets : Tag<ExportAssets>
{
    public ushort Count;
    public List<ExportedAsset> Assets { get; set; } = new();

    public ExportAssets(
        TagHeader header,
        ushort count,
        List<ExportedAsset> assets
    ) : base(header)
    {
        Count = count;
        Assets = [.. assets];
    }

    public static new ExportAssets Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var count = reader.ReadUInt16();
        var assets = new List<ExportedAsset>();
        for (int i = 0; i < count; i++)
        {
            var asset = new ExportedAsset
            {
                CharacterId = reader.ReadUInt16(), // UI16
                Name = reader.ReadNullTerminatedString(),
            };
            assets.Add(asset);
        }
        return new ExportAssets(
            header: tagHeader,
            count: count,
            assets: assets
        );
    }

}