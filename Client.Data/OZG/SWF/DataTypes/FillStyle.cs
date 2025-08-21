namespace Client.Data.OZG.SWF.DataTypes;

public class FillStyle
{
    public int FillStyleType;
    public bool InShape3;

    public RgbColor? Color;

    public Matrix? GradientMatrix;

    public Gradient? Gradient;
    public int BitmapId;
    public Matrix? BitmapMatrix;
    
}