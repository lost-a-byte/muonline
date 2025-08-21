using System.Text;

namespace Client.Data.OZG.SWF.Tags;

public class FileAttributes : Tag<FileAttributes>
{
    public int ReservedA;
    public int UseDirectBlit;
    public int UseGPU;
    public int HasMetadata;
    public int ActionScript3;
    public int NoCrossDomainCache;
    public int SwfRelativeUrls;
    public int UseNetwork;
    public uint ReservedB;

    public FileAttributes(
        TagHeader header,
        int reservedA,
        int useDirectBlit,
        int useGPU,
        int hasMetadata,
        int actionScript3,
        int noCrossDomainCache,
        int swfRelativeUrls,
        int useNetwork,
        uint reservedB
    ) : base(header)
    {
        ReservedA = reservedA;
        UseDirectBlit = useDirectBlit;
        UseGPU = useGPU;
        HasMetadata = hasMetadata;
        ActionScript3 = actionScript3;
        NoCrossDomainCache = noCrossDomainCache;
        SwfRelativeUrls = swfRelativeUrls;
        UseNetwork = useNetwork;
        ReservedB = reservedB;
    }

    public static new FileAttributes Parse(TagHeader tagHeader, byte[] buffer)
    {
        var br = new BitReader(buffer);

        int reservedA = (int)br.ReadUB(1);
        int useDirectBlit = (int)br.ReadUB(1);
        int useGPU = (int)br.ReadUB(1);
        int hasMetadata = (int)br.ReadUB(1);
        int actionScript3 = (int)br.ReadUB(1);
        int noCrossDomainCache = (int)br.ReadUB(1);
        int swfRelativeUrls = (int)br.ReadUB(1);
        var useNetwork = (int)br.ReadUB(1);

        // UB[24] == 0 (reserved)
        int bitCount = 24;
        if (br.AvailableBits < bitCount)
        {
            bitCount = br.AvailableBits;
        }
        var reservedB = br.ReadUB(bitCount);

        return new FileAttributes(
            header: tagHeader,
            reservedA: reservedA,
            useDirectBlit: useDirectBlit,
            useGPU: useGPU,
            hasMetadata: hasMetadata,
            actionScript3: actionScript3,
            noCrossDomainCache: noCrossDomainCache,
            swfRelativeUrls: swfRelativeUrls,
            useNetwork: useNetwork,
            reservedB: reservedB
        );
    }

}