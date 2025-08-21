using System;
using System.IO;
using System.Text;
using Client.Data.OZG.SWF.DataTypes;
using Client.Data.OZG.SWF.Tags;
using Org.BouncyCastle.Tls.Crypto.Impl;
namespace Client.Data.OZG.SWF.Extensions;

public static class BinaryReaderExtensions
{
    public static string ReadNullTerminatedString(this BinaryReader reader, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;

        using MemoryStream ms = new MemoryStream();
        byte b;
        while ((b = reader.ReadByte()) != 0)
        {
            ms.WriteByte(b);
        }

        return encoding.GetString(ms.ToArray());
    }

    public static Rect ReadRect(this BinaryReader reader)
    {
        byte firstByte = reader.ReadByte();
        int rectLength = Rect.ReadLength(firstByte);

        byte[] buffer = reader.ReadBytes(rectLength - 1);

        return Rect.Parse([firstByte, .. buffer]);
    }

    public static RgbaColor ReadRgbaColor(this BinaryReader reader)
    {
        return RgbaColor.Parse(reader.ReadBytes(4));
    }
    public static float ReadFloat(this BinaryReader reader)
    {
        int val = (int)reader.ReadUInt32();
        float ret = val.IntBitsToFloat();
        return ret;
    }

    public static TagHeader ReadTagHeader(this BinaryReader reader)
    {
        TagHeader tagHeader = TagHeader.Parse(reader);
        return tagHeader;
    }
    public static Tag ReadTag(this BinaryReader reader)
    {
        TagHeader tagHeader = reader.ReadTagHeader();
        byte[] payload = reader.ReadBytes(tagHeader.PayloadLength);

        Tag tag = tagHeader.TagCode switch
        {
            0 => End.Parse(tagHeader, payload),
            1 => ShowFrame.Parse(tagHeader, payload),
            9 => SetBackgroundColor.Parse(tagHeader, payload),
            12 => DoAction.Parse(tagHeader, payload),
            26 => PlaceObject2.Parse(tagHeader, payload),
            37 => DefineEditText.Parse(tagHeader, payload),
            // 39 => DefineSprite.Parse(tagHeader, payload),
            43 => FrameLabel.Parse(tagHeader, payload),
            56 => ExportAssets.Parse(tagHeader, payload),
            69 => Tags.FileAttributes.Parse(tagHeader, payload),
            71 => ImportAssets2.Parse(tagHeader, payload),
            74 => CSMTextSettings.Parse(tagHeader, payload),
            1000 => ExporterInfo.Parse(tagHeader, payload),
            1008 => DefineSubImage.Parse(tagHeader, payload),
            1009 => DefineExternalImage2.Parse(tagHeader, payload),
            _ => new Tag(tagHeader, payload) // throw new Exception($"Tag code: {tagHeader.TagCode} not implemented!"),
        };
        return tag;
    }

    public static Matrix ReadMatrix(this BinaryReader reader)
    {
        byte firstByte = reader.ReadByte();
        int nScaleBits = 0;
        float scaleX = 0;
        float scaleY = 0;
        int nRotateBits = 0;
        float rotateSkew0 = 0;
        float rotateSkew1 = 0;

        BitReader bitReader = new(firstByte);
        bool hasScale = bitReader.ReadUB();

        if (hasScale)
        {
            nScaleBits = (int)bitReader.ReadUB(5);
            while (bitReader.AvailableBits < nScaleBits * 2)
            {
                bitReader.Add(reader.ReadByte());
            }
            scaleX = bitReader.ReadFB(nScaleBits);
            scaleY = bitReader.ReadFB(nScaleBits);
        }

        if (bitReader.AvailableBits < 6)
        {
            bitReader.Add(reader.ReadByte());
        }
        bool hasRotate = bitReader.ReadUB();
        if (hasRotate)
        {
            nRotateBits = (int)bitReader.ReadUB(5);
            while (bitReader.AvailableBits < nRotateBits * 2)
            {
                bitReader.Add(reader.ReadByte());
            }
            rotateSkew0 = bitReader.ReadFB(nRotateBits);
            rotateSkew1 = bitReader.ReadFB(nRotateBits);
        }
        if (bitReader.AvailableBits < 5)
        {
            bitReader.Add(reader.ReadByte());
        }

        int nTranslateBits = (int)bitReader.ReadUB(5);
        while (bitReader.AvailableBits < nTranslateBits * 2)
        {
            bitReader.Add(reader.ReadByte());
        }
        int translateX = bitReader.ReadSB(nTranslateBits);
        int translateY = bitReader.ReadSB(nTranslateBits);
        return new Matrix
        {
            HasScale = hasScale,
            NScaleBits = nScaleBits,
            ScaleX = scaleX,
            ScaleY = scaleY,
            HasRotate = hasRotate,
            NRotateBits = nRotateBits,
            RotateSkew0 = rotateSkew0,
            RotateSkew1 = rotateSkew1,
            NTranslateBits = nTranslateBits,
            TranslateX = translateX,
            TranslateY = translateY,
        };
    }

    public static CxFormWithAlpha ReadCxFormWithAlpha(this BinaryReader reader)
    {
        byte firstByte = reader.ReadByte();

        BitReader bitReader = new(firstByte);
        bool hasAddTerms = bitReader.ReadUB();
        bool hasMultTerms = bitReader.ReadUB();
        int nBits = (int)bitReader.ReadUB(4);
        int redMultTerm = 0;
        int greenMultTerm = 0;
        int blueMultTerm = 0;
        int alphaMultTerm = 0;

        int redAddTerm = 0;
        int greenAddTerm = 0;
        int blueAddTerm = 0;
        int alphaAddTerm = 0;
        if (hasMultTerms)
        {
            while (bitReader.AvailableBits < nBits * 4)
            {
                bitReader.Add(reader.ReadByte());
            }
            redMultTerm = bitReader.ReadSB(nBits);
            greenMultTerm = bitReader.ReadSB(nBits);
            blueMultTerm = bitReader.ReadSB(nBits);
            alphaMultTerm = bitReader.ReadSB(nBits);
        }
        if (hasAddTerms)
        {
            while (bitReader.AvailableBits < nBits * 4)
            {
                bitReader.Add(reader.ReadByte());
            }
            redAddTerm = bitReader.ReadSB(nBits);
            greenAddTerm = bitReader.ReadSB(nBits);
            blueAddTerm = bitReader.ReadSB(nBits);
            alphaAddTerm = bitReader.ReadSB(nBits);
        }

        return new CxFormWithAlpha
        {
            HasAddTerms = hasAddTerms,
            HasMultTerms = hasMultTerms,
            NBits = nBits,
            RedMultTerm = redMultTerm,
            GreenMultTerm = greenMultTerm,
            BlueMultTerm = blueMultTerm,
            AlphaMultTerm = alphaMultTerm,
            RedAddTerm = redAddTerm,
            GreenAddTerm = greenAddTerm,
            BlueAddTerm = blueAddTerm,
            AlphaAddTerm = alphaAddTerm
        };
    }

    public static ClipActions ReadClipActions(this BinaryReader reader)
    {
        ushort reserved = reader.ReadUInt16();
        ClipEventFlags allEventFlags = reader.ReadClipEventFlags();

        List<ClipActionRecord> clipActionRecords = new();

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            ClipActionRecord record = reader.ReadClipActionRecord();
            clipActionRecords.Add(record);
        }

        return new()
        {
            Reserved = reserved,
            AllEventFlags = allEventFlags,
            ClipActionRecords = clipActionRecords
        };
    }

    public static ClipEventFlags ReadClipEventFlags(this BinaryReader reader)
    {
        byte[] payload = reader.ReadBytes(4);
        BitReader bitReader = new(payload);
        // Byte 1
        bool clipEventKeyUp = bitReader.ReadUB();
        bool clipEventKeyDown = bitReader.ReadUB();
        bool clipEventMouseUp = bitReader.ReadUB();
        bool clipEventMouseDown = bitReader.ReadUB();
        bool clipEventMouseMove = bitReader.ReadUB();
        bool clipEventUnload = bitReader.ReadUB();
        bool clipEventEnterFrame = bitReader.ReadUB();
        bool clipEventLoad = bitReader.ReadUB();
        // Byte 2
        bool clipEventDragOver = bitReader.ReadUB();
        bool clipEventRollOut = bitReader.ReadUB();
        bool clipEventRollOver = bitReader.ReadUB();
        bool clipEventReleaseOutside = bitReader.ReadUB();
        bool clipEventRelease = bitReader.ReadUB();
        bool clipEventPress = bitReader.ReadUB();
        bool clipEventInitialize = bitReader.ReadUB();
        bool clipEventData = bitReader.ReadUB();

        // Byte 3
        int reserved = (int)bitReader.ReadUB(5);
        bool clipEventConstruct = bitReader.ReadUB();
        bool clipEventKeyPress = bitReader.ReadUB();
        bool clipEventDragOut = bitReader.ReadUB();
        // Byte 4
        int reserved2 = (int)bitReader.ReadUB(8);

        return new()
        {
            ClipEventKeyUp = clipEventKeyUp,
            ClipEventKeyDown = clipEventKeyDown,
            ClipEventMouseUp = clipEventMouseUp,
            ClipEventMouseDown = clipEventMouseDown,
            ClipEventMouseMove = clipEventMouseMove,
            ClipEventUnload = clipEventUnload,
            ClipEventEnterFrame = clipEventEnterFrame,
            ClipEventLoad = clipEventLoad,
            ClipEventDragOver = clipEventDragOver,
            ClipEventRollOut = clipEventRollOut,
            ClipEventRollOver = clipEventRollOver,
            ClipEventReleaseOutside = clipEventReleaseOutside,
            ClipEventRelease = clipEventRelease,
            ClipEventPress = clipEventPress,
            ClipEventInitialize = clipEventInitialize,
            ClipEventData = clipEventData,
            Reserved = reserved,
            ClipEventConstruct = clipEventConstruct,
            ClipEventKeyPress = clipEventKeyPress,
            ClipEventDragOut = clipEventDragOut,
            Reserved2 = reserved2
        };
    }

    public static ClipActionRecord ReadClipActionRecord(this BinaryReader reader)
    {
        ClipEventFlags eventFlags = reader.ReadClipEventFlags();
        if (eventFlags.isClear())
        {
            // TODO: Skip this reading stream;
            return new()
            {
                EventFlags = eventFlags
            };
        }
        uint actionRecordSize = reader.ReadUInt32();
        byte keyCode = 0;
        if (eventFlags.ClipEventKeyPress)
        {
            keyCode = reader.ReadByte();
            actionRecordSize--;
        }
        byte[] actionBytes = reader.ReadBytes((int)actionRecordSize);

        return new()
        {
            EventFlags = eventFlags,
            ActionRecordSize = actionRecordSize,
            KeyCode = keyCode,
            ActionBytes = actionBytes,
        };
    }

    public static ShapeWithStyle ReadShapeWithStyle(this BinaryReader reader, int shapeNum = 1)
    {

    }

    public static FillStyleArray ReadFillStyleArray(this BinaryReader reader, int shapeNum = 1)
    {

    }
    public static LineStyleArray ReadLineStyleArray(this BinaryReader reader, int shapeNum = 1)
    {

    }



}
