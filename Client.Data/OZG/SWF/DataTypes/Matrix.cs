namespace Client.Data.OZG.SWF.DataTypes;

public class Matrix
{
    public bool HasScale;
    public float ScaleX;
    public float ScaleY;
    public int NScaleBits;

    public bool HasRotate;
    public float RotateSkew0;
    public float RotateSkew1;
    public int NRotateBits;

    public int TranslateX;
    public int TranslateY;
    public int NTranslateBits;

}