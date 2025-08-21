namespace Client.Data.OZG.SWF.DataTypes;

public class ClipActions
{
    public int Reserved;

    public ClipEventFlags AllEventFlags = new();

    public List<ClipActionRecord> ClipActionRecords = new();
}