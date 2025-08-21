namespace Client.Data.OZG.SWF.DataTypes;

public class Fixed8
{
    public static float Parse_LE(byte low, byte high)
    {
        short raw = (short)((high << 8) | low);
        int intPart = (sbyte)(raw >> 8);
        int fracPart = raw & 0xFF;
        return intPart + (fracPart / 256f);
    }
}