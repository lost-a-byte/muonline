namespace Client.Data.OZG.SWF.DataTypes;

public class Rect
{
    public int NBits;
    public int Xmin;
    public int Xmax;
    public int Ymin;
    public int Ymax;

    public static int ReadLength(byte data)
    {
        var reader = new BitReader([data]);

        var nBits = (int)reader.ReadUB(5);
        var totalBitCount = nBits * 4 + 5;

        return (int)Math.Ceiling((double)totalBitCount / 8);
    }
    public static Rect Parse(byte[] data)
    {
        var reader = new BitReader(data);
        var rect = new Rect();
        rect.NBits = (int)reader.ReadUB(5);
        rect.Xmin = reader.ReadSB(rect.NBits);
        rect.Xmax = reader.ReadSB(rect.NBits);
        rect.Ymin = reader.ReadSB(rect.NBits);
        rect.Ymax = reader.ReadSB(rect.NBits);
        return rect;
    }

    public override string ToString()
    {
        return $"NBits = {NBits}, Xmin = {Xmin}, Xmax = {Xmax}, Ymin = {Ymin}, Ymax = {Ymax}";
    }
}
