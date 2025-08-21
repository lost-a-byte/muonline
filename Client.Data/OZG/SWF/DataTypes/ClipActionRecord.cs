namespace Client.Data.OZG.SWF.DataTypes;

public class ClipActionRecord
{
    public ClipEventFlags EventFlags = new();
    public uint ActionRecordSize;
    public int KeyCode;
    public byte[] ActionBytes = [];
}