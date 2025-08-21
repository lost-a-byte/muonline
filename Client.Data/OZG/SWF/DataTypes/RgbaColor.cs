namespace Client.Data.OZG.SWF.DataTypes;

public class RgbaColor
{
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }
    public int Alpha { get; set; }

    public static RgbaColor Parse(byte[] data)
    {

        return new RgbaColor
        {
            Red = data[0],
            Green = data[1],
            Blue = data[2],
            Alpha = data[3],
        };
    }

    public override string ToString()
    {
        return $"red = {Red}, green = {Green}, blue = {Blue}, alpha = {Alpha}";
    }
}
