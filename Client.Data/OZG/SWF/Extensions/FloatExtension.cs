public static class FloatExtension
{
    public static float IntBitsToFloat(this int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return BitConverter.ToSingle(bytes, 0);
    }
}