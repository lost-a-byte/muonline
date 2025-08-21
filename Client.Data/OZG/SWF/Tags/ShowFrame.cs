using System.Text;

namespace Client.Data.OZG.SWF.Tags;

public class ShowFrame : Tag<ShowFrame>
{

    public ShowFrame(TagHeader header) : base(header)
    {
    }

    public static ShowFrame Parse(TagHeader tagHeader)
    {
        return new ShowFrame(tagHeader);
    }

    public static new ShowFrame Parse(TagHeader tagHeader, byte[] buffer)
    {
        return Parse(tagHeader);
    }

}