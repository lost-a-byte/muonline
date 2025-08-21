





using Client.Data.OZG.SWF.DataTypes;
using Client.Data.OZG.SWF.Extensions;


/**
placeFlagHasClipActions (UB) = 0
placeFlagHasClipDepth (UB) = 0
placeFlagHasName (UB) = 1
placeFlagHasRatio (UB) = 0
placeFlagHasColorTransform (UB) = 0
placeFlagHasMatrix (UB) = 1
placeFlagHasCharacter (UB) = 1
placeFlagMove (UB) = 0
*/
namespace Client.Data.OZG.SWF.Tags;

public class PlaceObject2(TagHeader header) : Tag<PlaceObject2>(header)
{
    public bool PlaceFlagHasClipActions;
    public bool PlaceFlagHasClipDepth;
    public bool PlaceFlagHasName;
    public bool PlaceFlagHasRatio;
    public bool PlaceFlagHasColorTransform;
    public bool PlaceFlagHasMatrix;
    public bool PlaceFlagHasCharacter;
    public bool PlaceFlagMove;
    public ushort Depth;
    public ushort CharacterId;
    public Matrix? Matrix = null;
    public CxFormWithAlpha? ColorTransform = null;
    public ushort Ratio;
    public string Name = "";
    public ushort ClipDepth;
    public ClipActions? ClipActions = null;


    public static new PlaceObject2 Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        byte configurationByte = reader.ReadByte();
        BitReader bitReader = new BitReader(configurationByte);

        bool placeFlagHasClipActions = (byte)bitReader.ReadUB(1) == 1;
        bool placeFlagHasClipDepth = (byte)bitReader.ReadUB(1) == 1;
        bool placeFlagHasName = (byte)bitReader.ReadUB(1) == 1;
        bool placeFlagHasRatio = (byte)bitReader.ReadUB(1) == 1;
        bool placeFlagHasColorTransform = (byte)bitReader.ReadUB(1) == 1;
        bool placeFlagHasMatrix = (byte)bitReader.ReadUB(1) == 1;
        bool placeFlagHasCharacter = (byte)bitReader.ReadUB(1) == 1;
        bool placeFlagMove = (byte)bitReader.ReadUB(1) == 1;

        ushort depth = reader.ReadUInt16();

        ushort characterId = 0;
        if (placeFlagHasCharacter)
        {
            characterId = reader.ReadUInt16();
        }
        Matrix? matrix = null;
        if (placeFlagHasMatrix)
        {
            matrix = reader.ReadMatrix();
        }

        CxFormWithAlpha? colorTransform = null;
        if (placeFlagHasColorTransform)
        {
            colorTransform = reader.ReadCxFormWithAlpha();
        }
        ushort ratio = 0;
        if (placeFlagHasRatio)
        {
            ratio = reader.ReadUInt16();
        }
        string name = "";
        if (placeFlagHasName)
        {
            name = reader.ReadNullTerminatedString();
        }

        ushort clipDepth = 0;
        if (placeFlagHasClipDepth)
        {
            clipDepth = reader.ReadUInt16();
        }

        ClipActions? clipActions = null;
        if (placeFlagHasClipActions)
        {
            clipActions = reader.ReadClipActions();
        }



        return new PlaceObject2(
            header: tagHeader
        )
        {
            PlaceFlagHasClipActions = placeFlagHasClipActions,
            PlaceFlagHasClipDepth = placeFlagHasClipDepth,
            PlaceFlagHasName = placeFlagHasName,
            PlaceFlagHasRatio = placeFlagHasRatio,
            PlaceFlagHasColorTransform = placeFlagHasColorTransform,
            PlaceFlagHasMatrix = placeFlagHasMatrix,
            PlaceFlagHasCharacter = placeFlagHasCharacter,
            PlaceFlagMove = placeFlagMove,
            Depth = depth,
            CharacterId = characterId,
            Matrix = matrix,
            ColorTransform = colorTransform,
            Ratio = ratio,
            Name = name,
            ClipDepth = clipDepth,
            ClipActions = clipActions,
        };
    }

}