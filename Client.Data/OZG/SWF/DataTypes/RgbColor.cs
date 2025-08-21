namespace Client.Data.OZG.SWF.DataTypes;

public class RgbColor
{
    public int red;
    public int green;
    public int blue;

    public static RgbColor Parse(byte[] data)
    {
        
        var color = new RgbColor();
        color.red = data[0];
        color.green = data[1];
        color.blue = data[2];
        return color;
    }

    public override string ToString()
    {
        return $"red = {red}, green = {green}, blue = {blue}";
    }
}
