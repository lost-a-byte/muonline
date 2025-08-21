using System.Text;
using Client.Data.OZG.SWF.DataTypes;

namespace Client.Data.OZG.SWF.Tags;

public class SetBackgroundColor : Tag<SetBackgroundColor>
{
    public RgbColor BackgroundColor;

    public SetBackgroundColor(TagHeader header, RgbColor backgroundColor) : base(header)
    {
        BackgroundColor = backgroundColor;
    }

    public static new SetBackgroundColor Parse(TagHeader tagHeader, byte[] buffer)
    {
        var backgroundColor = RgbColor.Parse(buffer);

        return new SetBackgroundColor(
            header: tagHeader,
            backgroundColor: backgroundColor
        );
    }

}