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
using System;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    // Describes a range of consecutive characters that should be included in the font.
    [TypeConverter(typeof(CharacterRegionTypeConverter))]
    public class CharacterRegion
    {
        // Constructor.
        public CharacterRegion(char start, char end)
        {
            if (start > end)
                throw new ArgumentException();

            this.Start = start;
            this.End = end;
        }


        // Fields.
        public char Start;
        public char End;


        // Enumerates all characters within the region.
        public IEnumerable<Char> Characters
        {
            get
            {
                for (char c = Start; c <= End; c++)
                {
                    yield return c;
                }
            }
        }


        // Flattens a list of character regions into a combined list of individual characters.
        public static IEnumerable<Char> Flatten(IEnumerable<CharacterRegion> regions)
        {
            if (regions.Any())
            {
                // If we have any regions, flatten them and remove duplicates.
                return regions.SelectMany(region => region.Characters).Distinct();
            }
            else
            {
                // If no regions were specified, use the default.
                return defaultRegion.Characters;
            }
        }

        
        // Default to just the base ASCII character set.
        static CharacterRegion defaultRegion = new CharacterRegion(' ', '~');
    }



    // Custom type converter enables CommandLineParser to parse CharacterRegion command line options.
    public class CharacterRegionTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // Input must be a string.
            string source = value as string;

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException();
            }

            // Supported input formats:
            //  A
            //  A-Z
            //  32-127
            //  0x20-0x7F

            char[] split = source.Split('-')
                                 .Select(ConvertCharacter)
                                 .ToArray();

            switch (split.Length)
            {
                case 1:
                    // Only a single character (eg. "a").
                    return new CharacterRegion(split[0], split[0]);

                case 2:
                    // Range of characters (eg. "a-z").
                    return new CharacterRegion(split[0], split[1]);
             
                default:
                    throw new ArgumentException();
            }
        }


        static char ConvertCharacter(string value)
        {
            if (value.Length == 1)
            {
                // Single character directly specifies a codepoint.
                return value[0];
            }
            else
            {
                // Otherwise it must be an integer (eg. "32" or "0x20").
                return (char)(int)intConverter.ConvertFromInvariantString(value);
            }
        }


        static TypeConverter intConverter = TypeDescriptor.GetConverter(typeof(int));
    }
}
