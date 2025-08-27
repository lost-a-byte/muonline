using System;

namespace Client.Data.Texture;

//
// Summary:
//     Defines types of surface formats.
public enum TextureSurfaceFormat
{
    //
    // Summary:
    //     Unsigned 32-bit ARGB pixel format for store 8 bits per channel.
    Color = 0,
    //
    // Summary:
    //     Unsigned 16-bit BGR pixel format for store 5 bits for blue, 6 bits for green,
    //     and 5 bits for red.
    Bgr565 = 1,
    //
    // Summary:
    //     Unsigned 16-bit BGRA pixel format where 5 bits reserved for each color and last
    //     bit is reserved for alpha.
    Bgra5551 = 2,
    //
    // Summary:
    //     Unsigned 16-bit BGRA pixel format for store 4 bits per channel.
    Bgra4444 = 3,
    //
    // Summary:
    //     DXT1. Texture format with compression. Surface dimensions must be a multiple
    //     4.
    Dxt1 = 4,
    //
    // Summary:
    //     DXT3. Texture format with compression. Surface dimensions must be a multiple
    //     4.
    Dxt3 = 5,
    //
    // Summary:
    //     DXT5. Texture format with compression. Surface dimensions must be a multiple
    //     4.
    Dxt5 = 6,
    //
    // Summary:
    //     Signed 16-bit bump-map format for store 8 bits for u and v data.
    NormalizedByte2 = 7,
    //
    // Summary:
    //     Signed 32-bit bump-map format for store 8 bits per channel.
    NormalizedByte4 = 8,
    //
    // Summary:
    //     Unsigned 32-bit RGBA pixel format for store 10 bits for each color and 2 bits
    //     for alpha.
    Rgba1010102 = 9,
    //
    // Summary:
    //     Unsigned 32-bit RG pixel format using 16 bits per channel.
    Rg32 = 10,
    //
    // Summary:
    //     Unsigned 64-bit RGBA pixel format using 16 bits per channel.
    Rgba64 = 11,
    //
    // Summary:
    //     Unsigned A 8-bit format for store 8 bits to alpha channel.
    Alpha8 = 12,
    //
    // Summary:
    //     IEEE 32-bit R float format for store 32 bits to red channel.
    Single = 13,
    //
    // Summary:
    //     IEEE 64-bit RG float format for store 32 bits per channel.
    Vector2 = 14,
    //
    // Summary:
    //     IEEE 128-bit RGBA float format for store 32 bits per channel.
    Vector4 = 15,
    //
    // Summary:
    //     Float 16-bit R format for store 16 bits to red channel.
    HalfSingle = 16,
    //
    // Summary:
    //     Float 32-bit RG format for store 16 bits per channel.
    HalfVector2 = 17,
    //
    // Summary:
    //     Float 64-bit ARGB format for store 16 bits per channel.
    HalfVector4 = 18,
    //
    // Summary:
    //     Float pixel format for high dynamic range data.
    HdrBlendable = 19,
    //
    // Summary:
    //     For compatibility with WPF D3DImage.
    Bgr32 = 20,
    //
    // Summary:
    //     For compatibility with WPF D3DImage.
    Bgra32 = 21,
    //
    // Summary:
    //     Unsigned 32-bit RGBA sRGB pixel format that supports 8 bits per channel.
    ColorSRgb = 30,
    //
    // Summary:
    //     Unsigned 32-bit sRGB pixel format that supports 8 bits per channel. 8 bits are
    //     unused.
    Bgr32SRgb = 31,
    //
    // Summary:
    //     Unsigned 32-bit sRGB pixel format that supports 8 bits per channel.
    Bgra32SRgb = 32,
    //
    // Summary:
    //     DXT1. sRGB texture format with compression. Surface dimensions must be a multiple
    //     of 4.
    Dxt1SRgb = 33,
    //
    // Summary:
    //     DXT3. sRGB texture format with compression. Surface dimensions must be a multiple
    //     of 4.
    Dxt3SRgb = 34,
    //
    // Summary:
    //     DXT5. sRGB texture format with compression. Surface dimensions must be a multiple
    //     of 4.
    Dxt5SRgb = 35,
    //
    // Summary:
    //     PowerVR texture compression format (iOS and Android).
    RgbPvrtc2Bpp = 50,
    //
    // Summary:
    //     PowerVR texture compression format (iOS and Android).
    RgbPvrtc4Bpp = 51,
    //
    // Summary:
    //     PowerVR texture compression format (iOS and Android).
    RgbaPvrtc2Bpp = 52,
    //
    // Summary:
    //     PowerVR texture compression format (iOS and Android).
    RgbaPvrtc4Bpp = 53,
    //
    // Summary:
    //     Ericcson Texture Compression (Android)
    RgbEtc1 = 60,
    //
    // Summary:
    //     DXT1 version where 1-bit alpha is used.
    Dxt1a = 70,
    //
    // Summary:
    //     ATC/ATITC compression (Android)
    RgbaAtcExplicitAlpha = 80,
    //
    // Summary:
    //     ATC/ATITC compression (Android)
    RgbaAtcInterpolatedAlpha = 81,
    //
    // Summary:
    //     Etc2 RGB8 (Android/iOS withh OpenglES 3.0)
    Rgb8Etc2 = 90,
    //
    // Summary:
    //     Etc2 SRGB8 (Android/iOS withh OpenglES 3.0)
    Srgb8Etc2 = 91,
    //
    // Summary:
    //     Etc2 RGB8A1 (Android/iOS withh OpenglES 3.0)
    Rgb8A1Etc2 = 92,
    //
    // Summary:
    //     Etc2 SRGB8A1 (Android/iOS withh OpenglES 3.0)
    Srgb8A1Etc2 = 93,
    //
    // Summary:
    //     Etc2 RGBA8 EAC (Android/iOS withh OpenglES 3.0)
    Rgba8Etc2 = 94,
    //
    // Summary:
    //     Etc2 SRGB8A8 EAC (Android/iOS withh OpenglES 3.0)
    SRgb8A8Etc2 = 95,
    //
    // Summary:
    //     Adaptive scalable texture compression ; 4x4 matrix using rgba channel interpretation
    Astc4X4Rgba = 96
}
