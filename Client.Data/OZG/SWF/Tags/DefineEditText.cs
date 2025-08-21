using Client.Data.OZG.SWF;
using Client.Data.OZG.SWF.DataTypes;
using Client.Data.OZG.SWF.Extensions;
using Client.Data.OZG.SWF.Tags;


/***
characterID (UI16) = 20
bounds (RECT)
NBits (UB) = 14
Xmin (SB) = -40
Xmax (SB) = 4980
Ymin (SB) = -40
Ymax (SB) = 960
hasText (UB) = 0
wordWrap (UB) = 1
multiline (UB) = 1
password (UB) = 0
readOnly (UB) = 1
hasTextColor (UB) = 1
hasMaxLength (UB) = 0
hasFont (UB) = 1
hasFontClass (UB) = 0
autoSize (UB) = 0
hasLayout (UB) = 1
noSelect (UB) = 1
border (UB) = 0
wasStatic (UB) = 0
html (UB) = 0
useOutlines (UB) = 1
fontId (UI16) = 19
fontHeight (UI16) = 240
textColor (RGBA)
align (UI8) = 2
leftMargin (UI16) = 0
rightMargin (UI16) = 0
indent (UI16) = 0
leading (SI16) = 40
variableName (string)
*/

class DefineEditText(TagHeader header) : Tag<DefineEditText>(header)
{
    public ushort CharacterID { get; set; }
    public required Rect Bounds { get; set; }
    public int HasText { get; set; }
    public int WordWrap { get; set; }
    public int Multiline { get; set; }
    public int Password { get; set; }
    public int ReadOnly { get; set; }
    public int HasTextColor { get; set; }
    public int HasMaxLength { get; set; }
    public int HasFont { get; set; }
    public int HasFontClass { get; set; }
    public ushort FontHeight { get; set; }
    public int AutoSize { get; set; }
    public int HasLayout { get; set; }
    public int NoSelect { get; set; }
    public int Border { get; set; }
    public int WasStatic { get; set; }
    public int Html { get; set; }
    public int UseOutlines { get; set; }
    public int FontId { get; set; }
    public string FontClass { get; set; } = "";
    public RgbaColor? TextColor { get; set; }
    public ushort? MaxLength { get; set; }
    public int? Align { get; set; }
    public int? LeftMargin { get; set; }
    public int? RightMargin { get; set; }
    public int? Indent { get; set; }
    public int? Leading { get; set; }
    public string VariableName { get; set; } = "";
    public string? InitialText { get; set; }

    public static new DefineEditText Parse(TagHeader tagHeader, byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        using var reader = new BinaryReader(stream);

        var characterID = reader.ReadUInt16();
        var bounds = reader.ReadRect();

        byte[] configurationBytes = reader.ReadBytes(2); // 2 byte => 16 bits
        var bitReader = new BitReader(configurationBytes);

        var hasText = (int)bitReader.ReadUB(1);
        var wordWrap = (int)bitReader.ReadUB(1);
        var multiline = (int)bitReader.ReadUB(1);
        var password = (int)bitReader.ReadUB(1);
        var readOnly = (int)bitReader.ReadUB(1);
        var hasTextColor = (int)bitReader.ReadUB(1);
        var hasMaxLength = (int)bitReader.ReadUB(1);
        var hasFont = (int)bitReader.ReadUB(1);
        var hasFontClass = (int)bitReader.ReadUB(1);
        var autoSize = (int)bitReader.ReadUB(1);
        var hasLayout = (int)bitReader.ReadUB(1);
        var noSelect = (int)bitReader.ReadUB(1);
        var border = (int)bitReader.ReadUB(1);
        var wasStatic = (int)bitReader.ReadUB(1);
        var html = (int)bitReader.ReadUB(1);
        var useOutlines = (int)bitReader.ReadUB(1);

        ushort fontId = 0;
        if (hasFont == 1)
        {
            fontId = reader.ReadUInt16();
        }

        string fontClass = "";
        if (hasFontClass == 1)
        {
            fontClass = reader.ReadNullTerminatedString();
        }

        ushort fontHeight = 0;
        if (hasFont == 1 || hasFontClass == 1)
        {
            fontHeight = reader.ReadUInt16();
        }

        RgbaColor? textColor = null;
        if (hasTextColor == 1)
        {
            textColor = reader.ReadRgbaColor();
        }

        ushort maxLength = 0;
        if (hasMaxLength == 1)
        {
            maxLength = reader.ReadUInt16();
        }

        byte align = 0;
        ushort leftMargin = 0;
        ushort rightMargin = 0;
        ushort indent = 0;
        short leading = 0;
        if (hasLayout == 1)
        {
            align = reader.ReadByte();
            leftMargin = reader.ReadUInt16();
            rightMargin = reader.ReadUInt16();
            indent = reader.ReadUInt16();
            leading = reader.ReadInt16();
        }

        string variableName = reader.ReadNullTerminatedString();
        string initialText = "";
        if (hasText == 1)
        {
            initialText = reader.ReadNullTerminatedString();
        }

        return new DefineEditText(tagHeader)
        {
            CharacterID = characterID,
            Bounds = bounds,
            HasText = hasText,
            WordWrap = wordWrap,
            Multiline = multiline,
            Password = password,
            ReadOnly = readOnly,
            HasTextColor = hasTextColor,
            HasMaxLength = hasMaxLength,
            HasFont = hasFont,
            HasFontClass = hasFontClass,
            AutoSize = autoSize,
            HasLayout = hasLayout,
            NoSelect = noSelect,
            Border = border,
            WasStatic = wasStatic,
            Html = html,
            UseOutlines = useOutlines,
            FontId = fontId,
            FontClass = fontClass,
            FontHeight = fontHeight,
            TextColor = textColor,
            MaxLength = maxLength,
            Align = align,
            LeftMargin = leftMargin,
            RightMargin = rightMargin,
            Indent = indent,
            Leading = leading,
            VariableName = variableName,
            InitialText = initialText,
        };
    }
}