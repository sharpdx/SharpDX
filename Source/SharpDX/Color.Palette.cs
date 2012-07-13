// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SharpDX
{
    /// <summary>
    /// List of predefined <see cref="Color"/>.
    /// </summary>
    public partial struct Color
    {
        /// <summary>
        /// Zero color.
        /// </summary>
        public static readonly Color Zero = new Color(0x00000000);

        /// <summary>
        /// Transparent color.
        /// </summary>
        public static readonly Color Transparent = new Color(0x00FFFFFF);

        /// <summary>
        /// AliceBlue color.
        /// </summary>
        public static readonly Color AliceBlue = new Color(0xFFF0F8FF);

        /// <summary>
        /// AntiqueWhite color.
        /// </summary>
        public static readonly Color AntiqueWhite = new Color(0xFFFAEBD7);

        /// <summary>
        /// Aqua color.
        /// </summary>
        public static readonly Color Aqua = new Color(0xFF00FFFF);

        /// <summary>
        /// Aquamarine color.
        /// </summary>
        public static readonly Color Aquamarine = new Color(0xFF7FFFD4);

        /// <summary>
        /// Azure color.
        /// </summary>
        public static readonly Color Azure = new Color(0xFFF0FFFF);

        /// <summary>
        /// Beige color.
        /// </summary>
        public static readonly Color Beige = new Color(0xFFF5F5DC);

        /// <summary>
        /// Bisque color.
        /// </summary>
        public static readonly Color Bisque = new Color(0xFFFFE4C4);

        /// <summary>
        /// Black color.
        /// </summary>
        public static readonly Color Black = new Color(0xFF000000);

        /// <summary>
        /// BlanchedAlmond color.
        /// </summary>
        public static readonly Color BlanchedAlmond = new Color(0xFFFFEBCD);

        /// <summary>
        /// Blue color.
        /// </summary>
        public static readonly Color Blue = new Color(0xFF0000FF);

        /// <summary>
        /// BlueViolet color.
        /// </summary>
        public static readonly Color BlueViolet = new Color(0xFF8A2BE2);

        /// <summary>
        /// Brown color.
        /// </summary>
        public static readonly Color Brown = new Color(0xFFA52A2A);

        /// <summary>
        /// BurlyWood color.
        /// </summary>
        public static readonly Color BurlyWood = new Color(0xFFDEB887);

        /// <summary>
        /// CadetBlue color.
        /// </summary>
        public static readonly Color CadetBlue = new Color(0xFF5F9EA0);

        /// <summary>
        /// Chartreuse color.
        /// </summary>
        public static readonly Color Chartreuse = new Color(0xFF7FFF00);

        /// <summary>
        /// Chocolate color.
        /// </summary>
        public static readonly Color Chocolate = new Color(0xFFD2691E);

        /// <summary>
        /// Coral color.
        /// </summary>
        public static readonly Color Coral = new Color(0xFFFF7F50);

        /// <summary>
        /// CornflowerBlue color.
        /// </summary>
        public static readonly Color CornflowerBlue = new Color(0xFF6495ED);

        /// <summary>
        /// Cornsilk color.
        /// </summary>
        public static readonly Color Cornsilk = new Color(0xFFFFF8DC);

        /// <summary>
        /// Crimson color.
        /// </summary>
        public static readonly Color Crimson = new Color(0xFFDC143C);

        /// <summary>
        /// Cyan color.
        /// </summary>
        public static readonly Color Cyan = new Color(0xFF00FFFF);

        /// <summary>
        /// DarkBlue color.
        /// </summary>
        public static readonly Color DarkBlue = new Color(0xFF00008B);

        /// <summary>
        /// DarkCyan color.
        /// </summary>
        public static readonly Color DarkCyan = new Color(0xFF008B8B);

        /// <summary>
        /// DarkGoldenrod color.
        /// </summary>
        public static readonly Color DarkGoldenrod = new Color(0xFFB8860B);

        /// <summary>
        /// DarkGray color.
        /// </summary>
        public static readonly Color DarkGray = new Color(0xFFA9A9A9);

        /// <summary>
        /// DarkGreen color.
        /// </summary>
        public static readonly Color DarkGreen = new Color(0xFF006400);

        /// <summary>
        /// DarkKhaki color.
        /// </summary>
        public static readonly Color DarkKhaki = new Color(0xFFBDB76B);

        /// <summary>
        /// DarkMagenta color.
        /// </summary>
        public static readonly Color DarkMagenta = new Color(0xFF8B008B);

        /// <summary>
        /// DarkOliveGreen color.
        /// </summary>
        public static readonly Color DarkOliveGreen = new Color(0xFF556B2F);

        /// <summary>
        /// DarkOrange color.
        /// </summary>
        public static readonly Color DarkOrange = new Color(0xFFFF8C00);

        /// <summary>
        /// DarkOrchid color.
        /// </summary>
        public static readonly Color DarkOrchid = new Color(0xFF9932CC);

        /// <summary>
        /// DarkRed color.
        /// </summary>
        public static readonly Color DarkRed = new Color(0xFF8B0000);

        /// <summary>
        /// DarkSalmon color.
        /// </summary>
        public static readonly Color DarkSalmon = new Color(0xFFE9967A);

        /// <summary>
        /// DarkSeaGreen color.
        /// </summary>
        public static readonly Color DarkSeaGreen = new Color(0xFF8FBC8B);

        /// <summary>
        /// DarkSlateBlue color.
        /// </summary>
        public static readonly Color DarkSlateBlue = new Color(0xFF483D8B);

        /// <summary>
        /// DarkSlateGray color.
        /// </summary>
        public static readonly Color DarkSlateGray = new Color(0xFF2F4F4F);

        /// <summary>
        /// DarkTurquoise color.
        /// </summary>
        public static readonly Color DarkTurquoise = new Color(0xFF00CED1);

        /// <summary>
        /// DarkViolet color.
        /// </summary>
        public static readonly Color DarkViolet = new Color(0xFF9400D3);

        /// <summary>
        /// DeepPink color.
        /// </summary>
        public static readonly Color DeepPink = new Color(0xFFFF1493);

        /// <summary>
        /// DeepSkyBlue color.
        /// </summary>
        public static readonly Color DeepSkyBlue = new Color(0xFF00BFFF);

        /// <summary>
        /// DimGray color.
        /// </summary>
        public static readonly Color DimGray = new Color(0xFF696969);

        /// <summary>
        /// DodgerBlue color.
        /// </summary>
        public static readonly Color DodgerBlue = new Color(0xFF1E90FF);

        /// <summary>
        /// Firebrick color.
        /// </summary>
        public static readonly Color Firebrick = new Color(0xFFB22222);

        /// <summary>
        /// FloralWhite color.
        /// </summary>
        public static readonly Color FloralWhite = new Color(0xFFFFFAF0);

        /// <summary>
        /// ForestGreen color.
        /// </summary>
        public static readonly Color ForestGreen = new Color(0xFF228B22);

        /// <summary>
        /// Fuchsia color.
        /// </summary>
        public static readonly Color Fuchsia = new Color(0xFFFF00FF);

        /// <summary>
        /// Gainsboro color.
        /// </summary>
        public static readonly Color Gainsboro = new Color(0xFFDCDCDC);

        /// <summary>
        /// GhostWhite color.
        /// </summary>
        public static readonly Color GhostWhite = new Color(0xFFF8F8FF);

        /// <summary>
        /// Gold color.
        /// </summary>
        public static readonly Color Gold = new Color(0xFFFFD700);

        /// <summary>
        /// Goldenrod color.
        /// </summary>
        public static readonly Color Goldenrod = new Color(0xFFDAA520);

        /// <summary>
        /// Gray color.
        /// </summary>
        public static readonly Color Gray = new Color(0xFF808080);

        /// <summary>
        /// Green color.
        /// </summary>
        public static readonly Color Green = new Color(0xFF008000);

        /// <summary>
        /// GreenYellow color.
        /// </summary>
        public static readonly Color GreenYellow = new Color(0xFFADFF2F);

        /// <summary>
        /// Honeydew color.
        /// </summary>
        public static readonly Color Honeydew = new Color(0xFFF0FFF0);

        /// <summary>
        /// HotPink color.
        /// </summary>
        public static readonly Color HotPink = new Color(0xFFFF69B4);

        /// <summary>
        /// IndianRed color.
        /// </summary>
        public static readonly Color IndianRed = new Color(0xFFCD5C5C);

        /// <summary>
        /// Indigo color.
        /// </summary>
        public static readonly Color Indigo = new Color(0xFF4B0082);

        /// <summary>
        /// Ivory color.
        /// </summary>
        public static readonly Color Ivory = new Color(0xFFFFFFF0);

        /// <summary>
        /// Khaki color.
        /// </summary>
        public static readonly Color Khaki = new Color(0xFFF0E68C);

        /// <summary>
        /// Lavender color.
        /// </summary>
        public static readonly Color Lavender = new Color(0xFFE6E6FA);

        /// <summary>
        /// LavenderBlush color.
        /// </summary>
        public static readonly Color LavenderBlush = new Color(0xFFFFF0F5);

        /// <summary>
        /// LawnGreen color.
        /// </summary>
        public static readonly Color LawnGreen = new Color(0xFF7CFC00);

        /// <summary>
        /// LemonChiffon color.
        /// </summary>
        public static readonly Color LemonChiffon = new Color(0xFFFFFACD);

        /// <summary>
        /// LightBlue color.
        /// </summary>
        public static readonly Color LightBlue = new Color(0xFFADD8E6);

        /// <summary>
        /// LightCoral color.
        /// </summary>
        public static readonly Color LightCoral = new Color(0xFFF08080);

        /// <summary>
        /// LightCyan color.
        /// </summary>
        public static readonly Color LightCyan = new Color(0xFFE0FFFF);

        /// <summary>
        /// LightGoldenrodYellow color.
        /// </summary>
        public static readonly Color LightGoldenrodYellow = new Color(0xFFFAFAD2);

        /// <summary>
        /// LightGray color.
        /// </summary>
        public static readonly Color LightGray = new Color(0xFFD3D3D3);

        /// <summary>
        /// LightGreen color.
        /// </summary>
        public static readonly Color LightGreen = new Color(0xFF90EE90);

        /// <summary>
        /// LightPink color.
        /// </summary>
        public static readonly Color LightPink = new Color(0xFFFFB6C1);

        /// <summary>
        /// LightSalmon color.
        /// </summary>
        public static readonly Color LightSalmon = new Color(0xFFFFA07A);

        /// <summary>
        /// LightSeaGreen color.
        /// </summary>
        public static readonly Color LightSeaGreen = new Color(0xFF20B2AA);

        /// <summary>
        /// LightSkyBlue color.
        /// </summary>
        public static readonly Color LightSkyBlue = new Color(0xFF87CEFA);

        /// <summary>
        /// LightSlateGray color.
        /// </summary>
        public static readonly Color LightSlateGray = new Color(0xFF778899);

        /// <summary>
        /// LightSteelBlue color.
        /// </summary>
        public static readonly Color LightSteelBlue = new Color(0xFFB0C4DE);

        /// <summary>
        /// LightYellow color.
        /// </summary>
        public static readonly Color LightYellow = new Color(0xFFFFFFE0);

        /// <summary>
        /// Lime color.
        /// </summary>
        public static readonly Color Lime = new Color(0xFF00FF00);

        /// <summary>
        /// LimeGreen color.
        /// </summary>
        public static readonly Color LimeGreen = new Color(0xFF32CD32);

        /// <summary>
        /// Linen color.
        /// </summary>
        public static readonly Color Linen = new Color(0xFFFAF0E6);

        /// <summary>
        /// Magenta color.
        /// </summary>
        public static readonly Color Magenta = new Color(0xFFFF00FF);

        /// <summary>
        /// Maroon color.
        /// </summary>
        public static readonly Color Maroon = new Color(0xFF800000);

        /// <summary>
        /// MediumAquamarine color.
        /// </summary>
        public static readonly Color MediumAquamarine = new Color(0xFF66CDAA);

        /// <summary>
        /// MediumBlue color.
        /// </summary>
        public static readonly Color MediumBlue = new Color(0xFF0000CD);

        /// <summary>
        /// MediumOrchid color.
        /// </summary>
        public static readonly Color MediumOrchid = new Color(0xFFBA55D3);

        /// <summary>
        /// MediumPurple color.
        /// </summary>
        public static readonly Color MediumPurple = new Color(0xFF9370DB);

        /// <summary>
        /// MediumSeaGreen color.
        /// </summary>
        public static readonly Color MediumSeaGreen = new Color(0xFF3CB371);

        /// <summary>
        /// MediumSlateBlue color.
        /// </summary>
        public static readonly Color MediumSlateBlue = new Color(0xFF7B68EE);

        /// <summary>
        /// MediumSpringGreen color.
        /// </summary>
        public static readonly Color MediumSpringGreen = new Color(0xFF00FA9A);

        /// <summary>
        /// MediumTurquoise color.
        /// </summary>
        public static readonly Color MediumTurquoise = new Color(0xFF48D1CC);

        /// <summary>
        /// MediumVioletRed color.
        /// </summary>
        public static readonly Color MediumVioletRed = new Color(0xFFC71585);

        /// <summary>
        /// MidnightBlue color.
        /// </summary>
        public static readonly Color MidnightBlue = new Color(0xFF191970);

        /// <summary>
        /// MintCream color.
        /// </summary>
        public static readonly Color MintCream = new Color(0xFFF5FFFA);

        /// <summary>
        /// MistyRose color.
        /// </summary>
        public static readonly Color MistyRose = new Color(0xFFFFE4E1);

        /// <summary>
        /// Moccasin color.
        /// </summary>
        public static readonly Color Moccasin = new Color(0xFFFFE4B5);

        /// <summary>
        /// NavajoWhite color.
        /// </summary>
        public static readonly Color NavajoWhite = new Color(0xFFFFDEAD);

        /// <summary>
        /// Navy color.
        /// </summary>
        public static readonly Color Navy = new Color(0xFF000080);

        /// <summary>
        /// OldLace color.
        /// </summary>
        public static readonly Color OldLace = new Color(0xFFFDF5E6);

        /// <summary>
        /// Olive color.
        /// </summary>
        public static readonly Color Olive = new Color(0xFF808000);

        /// <summary>
        /// OliveDrab color.
        /// </summary>
        public static readonly Color OliveDrab = new Color(0xFF6B8E23);

        /// <summary>
        /// Orange color.
        /// </summary>
        public static readonly Color Orange = new Color(0xFFFFA500);

        /// <summary>
        /// OrangeRed color.
        /// </summary>
        public static readonly Color OrangeRed = new Color(0xFFFF4500);

        /// <summary>
        /// Orchid color.
        /// </summary>
        public static readonly Color Orchid = new Color(0xFFDA70D6);

        /// <summary>
        /// PaleGoldenrod color.
        /// </summary>
        public static readonly Color PaleGoldenrod = new Color(0xFFEEE8AA);

        /// <summary>
        /// PaleGreen color.
        /// </summary>
        public static readonly Color PaleGreen = new Color(0xFF98FB98);

        /// <summary>
        /// PaleTurquoise color.
        /// </summary>
        public static readonly Color PaleTurquoise = new Color(0xFFAFEEEE);

        /// <summary>
        /// PaleVioletRed color.
        /// </summary>
        public static readonly Color PaleVioletRed = new Color(0xFFDB7093);

        /// <summary>
        /// PapayaWhip color.
        /// </summary>
        public static readonly Color PapayaWhip = new Color(0xFFFFEFD5);

        /// <summary>
        /// PeachPuff color.
        /// </summary>
        public static readonly Color PeachPuff = new Color(0xFFFFDAB9);

        /// <summary>
        /// Peru color.
        /// </summary>
        public static readonly Color Peru = new Color(0xFFCD853F);

        /// <summary>
        /// Pink color.
        /// </summary>
        public static readonly Color Pink = new Color(0xFFFFC0CB);

        /// <summary>
        /// Plum color.
        /// </summary>
        public static readonly Color Plum = new Color(0xFFDDA0DD);

        /// <summary>
        /// PowderBlue color.
        /// </summary>
        public static readonly Color PowderBlue = new Color(0xFFB0E0E6);

        /// <summary>
        /// Purple color.
        /// </summary>
        public static readonly Color Purple = new Color(0xFF800080);

        /// <summary>
        /// Red color.
        /// </summary>
        public static readonly Color Red = new Color(0xFFFF0000);

        /// <summary>
        /// RosyBrown color.
        /// </summary>
        public static readonly Color RosyBrown = new Color(0xFFBC8F8F);

        /// <summary>
        /// RoyalBlue color.
        /// </summary>
        public static readonly Color RoyalBlue = new Color(0xFF4169E1);

        /// <summary>
        /// SaddleBrown color.
        /// </summary>
        public static readonly Color SaddleBrown = new Color(0xFF8B4513);

        /// <summary>
        /// Salmon color.
        /// </summary>
        public static readonly Color Salmon = new Color(0xFFFA8072);

        /// <summary>
        /// SandyBrown color.
        /// </summary>
        public static readonly Color SandyBrown = new Color(0xFFF4A460);

        /// <summary>
        /// SeaGreen color.
        /// </summary>
        public static readonly Color SeaGreen = new Color(0xFF2E8B57);

        /// <summary>
        /// SeaShell color.
        /// </summary>
        public static readonly Color SeaShell = new Color(0xFFFFF5EE);

        /// <summary>
        /// Sienna color.
        /// </summary>
        public static readonly Color Sienna = new Color(0xFFA0522D);

        /// <summary>
        /// Silver color.
        /// </summary>
        public static readonly Color Silver = new Color(0xFFC0C0C0);

        /// <summary>
        /// SkyBlue color.
        /// </summary>
        public static readonly Color SkyBlue = new Color(0xFF87CEEB);

        /// <summary>
        /// SlateBlue color.
        /// </summary>
        public static readonly Color SlateBlue = new Color(0xFF6A5ACD);

        /// <summary>
        /// SlateGray color.
        /// </summary>
        public static readonly Color SlateGray = new Color(0xFF708090);

        /// <summary>
        /// Snow color.
        /// </summary>
        public static readonly Color Snow = new Color(0xFFFFFAFA);

        /// <summary>
        /// SpringGreen color.
        /// </summary>
        public static readonly Color SpringGreen = new Color(0xFF00FF7F);

        /// <summary>
        /// SteelBlue color.
        /// </summary>
        public static readonly Color SteelBlue = new Color(0xFF4682B4);

        /// <summary>
        /// Tan color.
        /// </summary>
        public static readonly Color Tan = new Color(0xFFD2B48C);

        /// <summary>
        /// Teal color.
        /// </summary>
        public static readonly Color Teal = new Color(0xFF008080);

        /// <summary>
        /// Thistle color.
        /// </summary>
        public static readonly Color Thistle = new Color(0xFFD8BFD8);

        /// <summary>
        /// Tomato color.
        /// </summary>
        public static readonly Color Tomato = new Color(0xFFFF6347);

        /// <summary>
        /// Turquoise color.
        /// </summary>
        public static readonly Color Turquoise = new Color(0xFF40E0D0);

        /// <summary>
        /// Violet color.
        /// </summary>
        public static readonly Color Violet = new Color(0xFFEE82EE);

        /// <summary>
        /// Wheat color.
        /// </summary>
        public static readonly Color Wheat = new Color(0xFFF5DEB3);

        /// <summary>
        /// White color.
        /// </summary>
        public static readonly Color White = new Color(0xFFFFFFFF);

        /// <summary>
        /// WhiteSmoke color.
        /// </summary>
        public static readonly Color WhiteSmoke = new Color(0xFFF5F5F5);

        /// <summary>
        /// Yellow color.
        /// </summary>
        public static readonly Color Yellow = new Color(0xFFFFFF00);

        /// <summary>
        /// YellowGreen color.
        /// </summary>
        public static readonly Color YellowGreen = new Color(0xFF9ACD32);
    }
}