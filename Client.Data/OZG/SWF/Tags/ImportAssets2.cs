
using Client.Data.OZG.SWF.Extensions;

namespace Client.Data.OZG.SWF.Tags;

public class ImportAsset2
{
    public ushort CharId;
    public required string TagName;
}
/**
url (string)
downloadNow (UI8) = 1
hasDigest (UI8) = 0
count (UI16) = 1
charId (UI16) = 19
tagName (string)
*/
public class ImportAssets2 : Tag<ImportAssets2>
{
    public string Url;
    public uint DownloadNow;
    public uint HasDigest;
    public uint Count;

    public List<ImportAsset2> Assets { get; set; } = new();
    public ImportAssets2(
        TagHeader header,
        string url,
        uint downloadNow,
        uint hasDigest,
        uint count,
        List<ImportAsset2> assets
    ) : base(header)
    {
        Url = url;
        DownloadNow = downloadNow;
        HasDigest = hasDigest;
        Count = count;
        Assets = [.. assets];
    }

    public static new ImportAssets2 Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var url = reader.ReadNullTerminatedString();
        var downloadNow = reader.ReadByte();
        var hasDigest = reader.ReadByte();
        var count = reader.ReadUInt16();
        var assets = new List<ImportAsset2>();
        for (int i = 0; i < count; i++)
        {
            var asset = new ImportAsset2
            {
                CharId = reader.ReadUInt16(), // UI16
                TagName = reader.ReadNullTerminatedString(),
            };
            assets.Add(asset);
        }
        return new ImportAssets2(
            header: tagHeader,
            url: url,
            downloadNow: downloadNow,
            hasDigest: hasDigest,
            count: count,
            assets: assets
        );
    }
}