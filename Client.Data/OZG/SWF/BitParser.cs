using System;
using System.IO;
using System.IO.Pipelines;


namespace Client.Data.OZG.SWF;

public class BitReader
{
    private byte[] data;
    private int bitPos = 0;

    public int AvailableBits
    {
        get
        {
            return data.Length * 8 - bitPos;
        }
    }

    public BitReader(byte[] data)
    {
        this.data = data;
    }
    public BitReader(byte data)
    {
        this.data = [data];
    }

    public void Add(byte singleByte)
    {
        data = [.. data, singleByte];
    }

    public uint ReadUB(int bitCount)
    {
        uint result = 0;
        for (int i = 0; i < bitCount; i++)
        {
            int byteIndex = bitPos / 8;
            int bitIndex = 7 - (bitPos % 8); // MSB-first
            int bit = (data[byteIndex] >> bitIndex) & 1;
            result = (result << 1) | (uint)bit;
            bitPos++;
        }
        return result;
    }

    public bool ReadUB()
    {
        return ReadUB(1) == 1;
    }

    public int ReadSB(int bitCount)
    {
        uint raw = ReadUB(bitCount);
        // Check if sign bit is set
        uint signBit = 1u << (bitCount - 1);
        if ((raw & signBit) != 0)
        {
            // Negative value
            int signed = (int)(raw | (~0u << bitCount)); // Sign-extend
            return signed;
        }
        else
        {
            return (int)raw;
        }
    }

    /**
     * Reads FB[nBits] (Signed fixed-point bit value) value from the stream.
     *
     * @param nBits Number of bits which represent value
     * @return Fixed-point value
     */
    public float ReadFB(int nBits)
    {
        if (nBits == 0)
        {
            return 0;
        }
        float val = ReadSB(nBits);
        float ret = val / 0x10000;
        return ret;
    }
}
