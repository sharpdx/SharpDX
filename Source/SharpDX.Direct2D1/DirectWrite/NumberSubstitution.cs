// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System;

namespace SharpDX.DirectWrite
{
    public partial class NumberSubstitution
    {
        /// <summary>	
        /// Creates a number substitution object using a locale name, substitution method, and an indicator  whether to ignore user overrides (use NLS defaults for the given culture instead). 	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="substitutionMethod">A value that specifies how to apply number substitution on digits and related punctuation. </param>
        /// <param name="localeName">The name of the locale to be used in the numberSubstitution object. </param>
        /// <param name="ignoreUserOverride">A Boolean flag that indicates whether to ignore user overrides. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateNumberSubstitution([In] DWRITE_NUMBER_SUBSTITUTION_METHOD substitutionMethod,[In] const wchar_t* localeName,[In] BOOL ignoreUserOverride,[Out] IDWriteNumberSubstitution** numberSubstitution)</unmanaged>
        public NumberSubstitution(Factory factory, NumberSubstitutionMethod substitutionMethod, string localeName, bool ignoreUserOverride)
        {
            factory.CreateNumberSubstitution(substitutionMethod, localeName, ignoreUserOverride, this);
        }
    }
}