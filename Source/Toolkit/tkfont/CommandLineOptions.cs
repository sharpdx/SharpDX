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
// -----------------------------------------------------------------------------
// The following code is a port of MakeSpriteFont from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//--------------------------------------------------------------------
using System.Collections.Generic;
using System.Drawing;

namespace SharpDX.Toolkit.Graphics
{
    // Available output texture formats.
    public enum TextureFormat
    {
        Auto,
        Rgba32,
        Bgra4444,
        CompressedMono,
    }


    // Options telling the tool what to do.
    public class CommandLineOptions
    {
        // Input can be either a system (TrueType) font or a specially marked bitmap file.
        [ConsoleProgram.OptionAttribute("SourceFont", Description = "Input can be either a system (TrueType) font or a specially marked bitmap file", Required = true, Value = "<fontname>")]
        public string SourceFont;


        // Output spritefont binary.
        [ConsoleProgram.OptionAttribute("OutputFile", Description = "Output spritefont binary", Required = true, Value = "<file>")]
        public string OutputFile;

        // Which characters to include in the font (eg. "/CharacterRegion:0x20-0x7F /CharacterRegion:0x123")
        [ConsoleProgram.OptionAttribute("CharacterRegion", Description = @"Which characters to include in the font (eg. ""/CharacterRegion:0x20-0x7F /CharacterRegion:0x123"")", Value = "<value>")]
        public readonly List<CharacterRegion> CharacterRegions = new List<CharacterRegion>();

        // Fallback character used when asked to render a codepoint that is not
        // included in the font. If zero, missing characters throw exceptions.
        [ConsoleProgram.OptionAttribute("DefaultCharacter", Description = "Fallback character used when asked to render a codepoint that is not included in the font", Value = "<integer>")]
        public int DefaultCharacter = 0;

        // Size and style for TrueType fonts (ignored when converting a bitmap font).
        [ConsoleProgram.OptionAttribute("FontSize", Description = "Size and style for TrueType fonts (ignored when converting a bitmap font)", Value = "<integer>")]
        public float FontSize = 23;

        // Font style
        [ConsoleProgram.OptionAttribute("FontStyle", Description = "Style for the font. 'regular', 'bold', 'italic', 'underline', 'strikeout'. Default is 'regular'\n", Value = "<style>")]
        public FontStyle FontStyle = FontStyle.Regular;

        // Spacing overrides. Zero is default spacing, negative closer together, positive further apart.
        [ConsoleProgram.OptionAttribute("LineSpacing", Description = "Line spacing overrides. Zero is default spacing, negative closer together, positive further apart", Value = "<float>")]
        public float LineSpacing = 0;

        [ConsoleProgram.OptionAttribute("CharacterSpacing", Description = "Character spacing overrides. Zero is default spacing, negative closer together, positive further apart\n", Value = "<float>")]
        public float CharacterSpacing = 0;

        // What format should the output texture be?
        [ConsoleProgram.OptionAttribute("TextureFormat", Description = "Format of the output texture. Values: 'auto', 'rgba32', 'bgra4444', 'compressedmono'. Default is 'auto'", Value = "<format>")]
        public TextureFormat TextureFormat = TextureFormat.Auto;

        // By default, font textures use premultiplied alpha format. Set this if you want interpolative alpha instead.
        [ConsoleProgram.OptionAttribute("NoPremultiply", Description = "By default, font textures use premultiplied alpha format. Set this if you want interpolative alpha instead\n")]
        public bool NoPremultiply = false;

        // Dumps the generated sprite texture to a bitmap file (useful for debugging).
        [ConsoleProgram.OptionAttribute("DebugOutputSpriteSheet", Description = "Dumps the generated sprite texture to a bitmap file (useful for debugging)", Value = "<output>")]
        public string DebugOutputSpriteSheet = null;
    }
}
